using edziennik.Models;
using edziennik.Resources;
using edziennik.Validators;
using Microsoft.AspNet.Identity;
using Models.Models;
using Repositories.Repositories;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace edziennik.Controllers
{
    [Authorize(Roles = "Teachers")]
    public class MarksController : Controller
    {
        private readonly MarkRepository markRepo;
        private readonly StudentRepository studentRepo;
        private readonly TeacherRepository teacherRepo;
        private readonly SubjectRepository subjectRepo;
        private readonly ClasssRepository classRepo;

        public MarksController(MarkRepository _markRepo, StudentRepository _studentRepo, 
                               TeacherRepository _teacherRepo, SubjectRepository _subjectRepo,
                               ClasssRepository _classRepo)
        {
            markRepo = _markRepo;
            studentRepo = _studentRepo;
            teacherRepo = _teacherRepo;
            subjectRepo = _subjectRepo;
            classRepo = _classRepo;
        }

        // GET: Marks
        [Authorize(Roles = "Teachers,Admins")]
        public ActionResult Index()
        {
            var marks = markRepo.GetAll().Select(a=> new MarkListItemViewModel
                {
                    Student = studentRepo.FindById(a.StudentId).FullName,
                    Teacher = teacherRepo.FindById(a.TeacherId).FullName,
                    Subject = subjectRepo.FindById(a.SubjectId).Name,
                    Value   = a.Value,
                    Classs  = classRepo.FindByMarkId(a.Id).Name,
                    Id = a.Id,
                    TeacherId = a.TeacherId                   
                });
           
            return View(marks);
        }
       
        [Authorize(Roles = "Students, Teachers, Admins")]
        public ActionResult StudentSubjectMarks(string studentId, int subjectId)
        {
            var marks = markRepo.FindByStudentIdAndSubjectId
                                (studentId,subjectId).Select(a => new MarkListItemViewModel
            {
                Student = studentRepo.FindById(a.StudentId).FullName,
                Teacher = teacherRepo.FindById(a.TeacherId).FullName,
                Subject = subjectRepo.FindById(a.SubjectId).Name,
                Value = a.Value,
                Classs = classRepo.FindByMarkId(a.Id).Name,
                Id = a.Id,
                TeacherId = a.TeacherId
            });

            return View(marks);
        }

        [Authorize(Roles = "Teachers,Admins")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mark mark = markRepo.FindById((int)id);
            if (mark == null)
            {
                return HttpNotFound();
            }

            var markVm = new MarkDetailsViewModel
            {
                Student = studentRepo.FindById(mark.StudentId).FullName,
                Teacher = teacherRepo.FindById(mark.TeacherId).FullName,
                Subject = subjectRepo.FindById(mark.SubjectId).Name,
                Value = mark.Value,
                Classs = classRepo.FindByMarkId(mark.Id).Name,
                Id = mark.Id,
                TeacherId = mark.TeacherId,
                Description = mark.Description
            };

            return View(markVm);
        }

        // GET: Marks/Create
        public ActionResult Create(string studentId)
        {
            if (studentId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var student = studentRepo.FindById(studentId);
            if (student == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var subjects = ConstantStrings.getStudentSubjectsSL(student.ClasssId, User.Identity.GetUserId());
            if(!subjects.Any())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var markVm = new MarkCreateViewModel
            {
                StudentId = studentId,
                TeacherId = User.Identity.GetUserId(),
                Subjects = subjects,
                Values = ConstantStrings.getMarksSL()
            };

            return View(markVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MarkCreateViewModel markVm)
        {
            if (ModelState.IsValid)
            {
                var mark = new Mark
                {
                    Description = markVm.Description,
                    Id = markVm.Id,
                    StudentId = markVm.StudentId,
                    SubjectId = markVm.SubjectId,
                    TeacherId = markVm.TeacherId,
                    Value = markVm.Value
                };

                markRepo.Insert(mark);
                markRepo.Save();
                if (studentRepo.FindById(markVm.StudentId).CellPhoneNumber != null)
                {
                    var number = studentRepo.FindById(markVm.StudentId).CellPhoneNumber;
                    SmsSender.SendSms(markVm,number);
                }
                Logs.SaveLog("Create", User.Identity.GetUserId(), 
                             "Mark", mark.Id.ToString(), Request.UserHostAddress);

                return RedirectToAction("Index");
            }
           
            return View(markVm);
        }


        [OnlyMarkTeacherOrAdmin]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mark mark = markRepo.FindById((int)id);
            if (mark == null)
            {
                return HttpNotFound();
            }
            var student = studentRepo.FindById(mark.StudentId);

            if (student == null)
            {
                return HttpNotFound();
            }

            var markVm = new MarkCreateViewModel
            {
                StudentId = mark.StudentId,
                TeacherId = User.Identity.GetUserId(),
                Subjects = ConstantStrings.getStudentSubjectsSL(student.ClasssId, User.Identity.GetUserId()),
                Values = ConstantStrings.getMarksSL(),
                Description = mark.Description
            };

            return View(markVm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MarkCreateViewModel markVm)
        {
            if (ModelState.IsValid)
            {
                var mark = new Mark
                {
                    Description = markVm.Description,
                    Id = markVm.Id,
                    StudentId = markVm.StudentId,
                    SubjectId = markVm.SubjectId,
                    TeacherId = markVm.TeacherId,
                    Value = markVm.Value
                };

                markRepo.Update(mark);
                markRepo.Save();
                Logs.SaveLog("Edit", User.Identity.GetUserId(),
                             "Mark", mark.Id.ToString(), Request.UserHostAddress);
                return RedirectToAction("Index");
            }

            return View(markVm);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mark mark = markRepo.FindById((int)id);
            if (mark == null)
            {
                return HttpNotFound();
            }

            var markVm = new MarkDetailsViewModel
            {
                Student = studentRepo.FindById(mark.StudentId).FullName,
                Teacher = teacherRepo.FindById(mark.TeacherId).FullName,
                Subject = subjectRepo.FindById(mark.SubjectId).Name,
                Value = mark.Value,
                Classs = classRepo.FindByMarkId(mark.Id).Name,
                Id = mark.Id,
                TeacherId= mark.TeacherId,
                Description = mark.Description
            };
            
            return View(markVm);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            markRepo.Delete(id);
            markRepo.Save();
            Logs.SaveLog("Delete", User.Identity.GetUserId(),
                         "Mark", id.ToString(), Request.UserHostAddress);
            
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            markRepo.Dispose();
            base.Dispose(disposing);
        }
    }
}
