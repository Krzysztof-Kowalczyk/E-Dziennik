using edziennik.Models;
using edziennik.Resources;
using Microsoft.AspNet.Identity;
using Models.Models;
using Repositories.Repositories;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using edziennik.Models.ViewModels;

namespace edziennik.Controllers
{
    public enum SubjectCreateError
    {
        NoClasses = 1,
        NoClassrooms = 2,
        NoTeachers = 3
    }
    
    public class SubjectsController : Controller
    {
        private readonly SubjectRepository subjectRepo;
        private readonly ClasssRepository classRepo;
        private readonly ClassroomRepository classroomRepo;
        private readonly TeacherRepository teacherRepo;

        public SubjectsController(SubjectRepository sr, ClasssRepository _classsRepo,
                                  ClassroomRepository _classroomRepo, TeacherRepository _teacherRepo)
        {
            subjectRepo = sr;
            classRepo = _classsRepo;
            classroomRepo = _classroomRepo;
            teacherRepo = _teacherRepo;
        }

        // GET: Subjects
        [Authorize(Roles = "Admins")]
        public ActionResult Index(SubjectCreateError? error)
        {
            if (error == SubjectCreateError.NoTeachers)
                ViewBag.Error = ConstantStrings.SubjectCreateNoTeachersError;
            else if (error == SubjectCreateError.NoClasses)
                ViewBag.Error = ConstantStrings.SubjectCreateNoClassesError;
            else if (error == SubjectCreateError.NoClassrooms)
                ViewBag.Error = ConstantStrings.SubjectCreateNoClassroomsError;

            var subjects = subjectRepo.GetAll().Select(a => new SubjectViewModel
            {
                Id = a.Id,
                Classroom = classroomRepo.FindById(a.ClassroomId).Name,
                Classs = classRepo.FindById(a.ClasssId).Name,
                Day = a.Day,
                Hour = a.Hour,
                Name = a.Name,
                Teacher = teacherRepo.FindById(a.TeacherId).FullName
            });
            
            return View(subjects);
        }

        [Authorize(Roles = "Admins,Teachers")]
        public ActionResult TeacherSubjects(string teacherId)
        {
            var subjects = subjectRepo.FindByTeacherId(teacherId).Select(a => new SubjectViewModel
            {
                Id = a.Id,
                Classroom = classroomRepo.FindById(a.ClassroomId).Name,
                Classs = classRepo.FindById(a.ClasssId).Name,
                Day = a.Day,
                Hour = a.Hour,
                Name = a.Name,
                Teacher = teacherRepo.FindById(a.TeacherId).FullName
            });

            return View("Index", subjects);
        }

        [Authorize(Roles = "Students,Admins,Teachers")]
        public ActionResult StudentSubjects(string studentId)
        {
            var subjects = subjectRepo.FindByStudentId(studentId).Select(a => new SubjectViewModel
            {
                Id = a.Id,
                Classroom = classroomRepo.FindById(a.ClassroomId).Name,
                Classs = classRepo.FindById(a.ClasssId).Name,
                Day = a.Day,
                Hour = a.Hour,
                Name = a.Name,
                Teacher = teacherRepo.FindById(a.TeacherId).FullName
            });

            return View("Index", subjects);
        }

        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var subject = subjectRepo.FindById((int) id);

            if (subject == null)
            {
                return HttpNotFound();
            }
            var subjectVm = new SubjectViewModel
            {
                Id = subject.Id,
                Classroom = classroomRepo.FindById(subject.ClassroomId).Name,
                Classs = classRepo.FindById(subject.ClasssId).Name,
                Day = subject.Day,
                Hour = subject.Hour,
                Name = subject.Name,
                Teacher = teacherRepo.FindById(subject.TeacherId).FullName
            };

            return View(subjectVm);
        }
        
        [Authorize(Roles = "Admins")]
        public ActionResult Create()
        {
            if(teacherRepo.GetAll().Count == 0)
            {
                return RedirectToAction("Index", new{ error= SubjectCreateError.NoTeachers});
            }
            if (classRepo.GetAll().Count == 0)
            {
                return RedirectToAction("Index", new { error = SubjectCreateError.NoClasses });
            }
            if (classroomRepo.GetAll().Count == 0)
            {
                return RedirectToAction("Index", new { error = SubjectCreateError.NoClassrooms });
            }

            var subjectVm = new SubjectCreateViewModel
            {
                Classes = ConstantStrings.getClassesSL(),
                Classrooms = ConstantStrings.getClassroomsSL(),
                Days = ConstantStrings.getSchoolDaysSL(),
                Hours = ConstantStrings.getSchoolHoursSL(),
                Teachers = ConstantStrings.getTeachersSL()
            };

            return View(subjectVm);
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admins")]
        public ActionResult Create(SubjectCreateViewModel subjectVm)
        {
            if (ModelState.IsValid)
            {
                var subject = new Subject
                {
                    ClassroomId = subjectVm.ClassroomId,
                    ClasssId = subjectVm.ClasssId,
                    Day = subjectVm.Day,
                    Hour = subjectVm.Hour,
                    Id = subjectVm.Id,
                    Name = subjectVm.Name,
                    TeacherId = subjectVm.TeacherId
                };

                subjectRepo.Insert(subject);
                subjectRepo.Save();
                Logs.SaveLog("Create", User.Identity.GetUserId(), 
                             "Subject", subject.Id.ToString(), Request.UserHostAddress);
                return RedirectToAction("Index");
            }

            return View(subjectVm);
        }

        [Authorize(Roles = "Admins")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = subjectRepo.FindById((int) id);
            if (subject == null)
            {
                return HttpNotFound();
            }

            var subjectVm = new SubjectCreateViewModel
            {
                Classes = ConstantStrings.getClassesSL(),
                Classrooms = ConstantStrings.getClassroomsSL(),
                Days = ConstantStrings.getSchoolDaysSL(),
                Hours = ConstantStrings.getSchoolHoursSL(),
                Teachers = ConstantStrings.getTeachersSL(),
                ClassroomId = subject.ClassroomId,
                ClasssId = subject.ClasssId,
                Day = subject.Day,
                Hour = subject.Hour,
                Id = subject.Id,
                TeacherId = subject.TeacherId,
                Name = subject.Name
            };

            return View(subjectVm);
        }

        [Authorize(Roles = "Admins")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SubjectCreateViewModel subjectVm)
        {
            if (ModelState.IsValid)
            {
                var subject = new Subject
                {
                    ClassroomId = subjectVm.ClassroomId,
                    ClasssId = subjectVm.ClasssId,
                    Day = subjectVm.Day,
                    Hour = subjectVm.Hour,
                    Id = subjectVm.Id,
                    Name = subjectVm.Name,
                    TeacherId = subjectVm.TeacherId
                };

                subjectRepo.Update(subject);
                subjectRepo.Save();
                Logs.SaveLog("Edit", User.Identity.GetUserId(), 
                             "Subject", subject.Id.ToString(), Request.UserHostAddress);
                return RedirectToAction("Index");
            }

            return View(subjectVm);
        }

        [Authorize(Roles = "Admins")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = subjectRepo.FindById((int) id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        [Authorize(Roles = "Admins")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            subjectRepo.Delete(id);
            subjectRepo.Save();
            Logs.SaveLog("Delete", User.Identity.GetUserId(),
                         "Subject", id.ToString(), Request.UserHostAddress);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            subjectRepo.Dispose();
            base.Dispose(disposing);
        }
    }
}
