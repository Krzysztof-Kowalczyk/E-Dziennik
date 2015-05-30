using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using edziennik.Models.ViewModels;
using edziennik.Resources;
using edziennik.Validators;
using Microsoft.AspNet.Identity;
using Models.Models;
using PagedList;
using Repositories.Repositories;

namespace edziennik.Controllers
{
    [Authorize(Roles = "Teachers")]
    public class MarksController : Controller
    {
        private readonly MarkRepository _markRepo;
        private readonly StudentRepository _studentRepo;
        private readonly TeacherRepository _teacherRepo;
        private readonly SubjectRepository _subjectRepo;
        private readonly ClasssRepository _classRepo;

        public MarksController(MarkRepository markRepo, StudentRepository studentRepo, 
                               TeacherRepository teacherRepo, SubjectRepository subjectRepo,
                               ClasssRepository classRepo)
        {
            _markRepo = markRepo;
            _studentRepo = studentRepo;
            _teacherRepo = teacherRepo;
            _subjectRepo = subjectRepo;
            _classRepo = classRepo;
        }

        // GET: Marks
        [Authorize(Roles = "Teachers,Admins")]
        public ActionResult Index(int? page, string sortOrder)
        {
            int currentPage = page ?? 1;
            var items = SortItems(sortOrder);

            var marks = items.ToList().Select(a=> new MarkListItemViewModel
                {
                    Student = _studentRepo.FindById(a.StudentId).FullName,
                    Teacher = _teacherRepo.FindById(a.TeacherId).FullName,
                    Subject = _subjectRepo.FindById(a.SubjectId).Name,
                    Value   = a.Value,
                    Classs  = _classRepo.FindByMarkId(a.Id).Name,
                    Id = a.Id,
                    TeacherId = a.TeacherId
                }).ToPagedList(currentPage, 10);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_MarkList", marks);
            }
           
            return View(marks);
        }

        [NonAction]
        private IQueryable<Mark> SortItems(string sortOrder)
        {
            var items = _markRepo.GetAll();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.IdSort = String.IsNullOrEmpty(sortOrder) ? "IdAsc" : "";
            ViewBag.StudentSort = sortOrder == "StudentAsc" ? "Student" : "StudentAsc";
            ViewBag.SubjectSort = sortOrder == "SubjectAsc" ? "Subject" : "SubjectAsc";
            ViewBag.TeacherSort = sortOrder == "TeacherAsc" ? "Teacher" : "TeacherAsc";
            ViewBag.ValueSort = sortOrder == "ValueAsc" ? "Value" : "ValueAsc";

            switch (sortOrder)
            {
                case "Student":
                    items = items.OrderByDescending(s => s.StudentId);
                    break;
                case "StudentAsc":
                    items = items.OrderBy(s => s.StudentId);
                    break;
                case "Subject":
                    items = items.OrderByDescending(s => s.SubjectId);
                    break;
                case "SubjectAsc":
                    items = items.OrderBy(s => s.SubjectId);
                    break;
                case "Teacher":
                    items = items.OrderByDescending(s => s.TeacherId);
                    break;
                case "TeacherAsc":
                    items = items.OrderBy(s => s.TeacherId);
                    break;
                case "Value":
                    items = items.OrderByDescending(s => s.Value);
                    break;
                case "ValueAsc":
                    items = items.OrderBy(s => s.Value);
                    break;
                case "IdAsc":
                    items = items.OrderBy(s => s.Id);
                    break;
                default:    // id descending
                    items = items.OrderByDescending(s => s.Id);
                    break;
            }
            return items;
        }
       
        [Authorize(Roles = "Students, Teachers, Admins")]
        public ActionResult StudentSubjectMarks(string studentId, int subjectId)
        {
            var marks = _markRepo.FindByStudentIdAndSubjectId
                                (studentId,subjectId).Select(a => new MarkListItemViewModel
            {
                Student = _studentRepo.FindById(a.StudentId).FullName,
                Teacher = _teacherRepo.FindById(a.TeacherId).FullName,
                Subject = _subjectRepo.FindById(a.SubjectId).Name,
                Value = a.Value,
                Classs = _classRepo.FindByMarkId(a.Id).Name,
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
            Mark mark = _markRepo.FindById((int)id);
            if (mark == null)
            {
                return HttpNotFound();
            }

            var markVm = new MarkDetailsViewModel
            {
                Student = _studentRepo.FindById(mark.StudentId).FullName,
                Teacher = _teacherRepo.FindById(mark.TeacherId).FullName,
                Subject = _subjectRepo.FindById(mark.SubjectId).Name,
                Value = mark.Value,
                Classs = _classRepo.FindByMarkId(mark.Id).Name,
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
            var student = _studentRepo.FindById(studentId);
            if (student == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var subjects = ConstantStrings.GetStudentSubjectsSl(student.ClasssId, User.Identity.GetUserId());
            if(!subjects.Any())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var markVm = new MarkCreateViewModel
            {
                StudentId = studentId,
                TeacherId = User.Identity.GetUserId(),
                Subjects = subjects,
                Values = ConstantStrings.GetMarksSl()
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

                _markRepo.Insert(mark);
                _markRepo.Save();
                if (_studentRepo.FindById(markVm.StudentId).CellPhoneNumber != null)
                {
                    var number = _studentRepo.FindById(markVm.StudentId).CellPhoneNumber;
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
            Mark mark = _markRepo.FindById((int)id);
            if (mark == null)
            {
                return HttpNotFound();
            }
            var student = _studentRepo.FindById(mark.StudentId);

            if (student == null)
            {
                return HttpNotFound();
            }

            var markVm = new MarkCreateViewModel
            {
                StudentId = mark.StudentId,
                TeacherId = User.Identity.GetUserId(),
                Subjects = ConstantStrings.GetStudentSubjectsSl(student.ClasssId, User.Identity.GetUserId()),
                Values = ConstantStrings.GetMarksSl(),
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

                _markRepo.Update(mark);
                _markRepo.Save();
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
            Mark mark = _markRepo.FindById((int)id);
            if (mark == null)
            {
                return HttpNotFound();
            }

            var markVm = new MarkDetailsViewModel
            {
                Student = _studentRepo.FindById(mark.StudentId).FullName,
                Teacher = _teacherRepo.FindById(mark.TeacherId).FullName,
                Subject = _subjectRepo.FindById(mark.SubjectId).Name,
                Value = mark.Value,
                Classs = _classRepo.FindByMarkId(mark.Id).Name,
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
            _markRepo.Delete(id);
            _markRepo.Save();
            Logs.SaveLog("Delete", User.Identity.GetUserId(),
                         "Mark", id.ToString(), Request.UserHostAddress);
            
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _markRepo.Dispose();
            base.Dispose(disposing);
        }
    }
}
