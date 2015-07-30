using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using edziennik.Models.ViewModels;
using edziennik.Resources;
using Microsoft.AspNet.Identity;
using Models.Models;
using PagedList;
using Repositories.Repositories;

namespace edziennik.Controllers
{
    public enum SubjectCreateError
    {
        NoClasses = 1,
        NoClassrooms = 2,
        NoTeachers = 3
    }
   
    [HandleError]
    public class SubjectsController : Controller
    {
        private readonly SubjectRepository _subjectRepo;
        private readonly ClasssRepository _classRepo;
        private readonly ClassroomRepository _classroomRepo;
        private readonly TeacherRepository _teacherRepo;
        private readonly StudentRepository _studentRepo;

        public SubjectsController(SubjectRepository subjectRepo, ClasssRepository classRepo,
                                  ClassroomRepository classroomRepo, TeacherRepository teacherRepo,
                                  StudentRepository studentRepository)
        {
            _subjectRepo = subjectRepo;
            _classRepo = classRepo;
            _classroomRepo = classroomRepo;
            _teacherRepo = teacherRepo;
            _studentRepo = studentRepository;
        }

        // GET: Subjects
        [Authorize(Roles = "Admins")]
        public ActionResult Index(int? page, SubjectCreateError? error, string sortOrder)
        {
            if (error == SubjectCreateError.NoTeachers)
                ViewBag.Error = ConstantStrings.SubjectCreateNoTeachersError;
            else if (error == SubjectCreateError.NoClasses)
                ViewBag.Error = ConstantStrings.SubjectCreateNoClassesError;
            else if (error == SubjectCreateError.NoClassrooms)
                ViewBag.Error = ConstantStrings.SubjectCreateNoClassroomsError;

            int currentPage = page ?? 1;
            var items = _subjectRepo.GetAll();

            items = SortItems(sortOrder, items);

            var subjects = items.ToList().Select(a => new SubjectViewModel
            {
                Id = a.Id,
                Classroom = _classroomRepo.FindById(a.ClassroomId).Name,
                Classs = _classRepo.FindById(a.ClasssId).Name,
                Day = a.Day,
                Hour = a.Hour,
                Name = a.Name,
                Teacher = _teacherRepo.FindById(a.TeacherId).FullName
            }).ToPagedList(currentPage, 10);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_SubjectList", subjects);
            }

            return View(subjects);
        }

        [NonAction]
        private IQueryable<Subject> SortItems(string sortOrder, IQueryable<Subject> items)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.IdSort = String.IsNullOrEmpty(sortOrder) ? "IdAsc" : "";
            ViewBag.ClassroomSort = sortOrder == "ClassroomAsc" ? "Classroom" : "ClassroomAsc";
            ViewBag.ClassSort = sortOrder == "ClassAsc" ? "Class" : "ClassAsc";
            ViewBag.DaySort = sortOrder == "DayAsc" ? "Day" : "DayAsc";
            ViewBag.HourSort = sortOrder == "HourAsc" ? "Hour" : "HourAsc";
            ViewBag.NameSort = sortOrder == "NameAsc" ? "Name" : "NameAsc";
            ViewBag.TeacherSort = sortOrder == "TeacherAsc" ? "Teacher" : "TeacherAsc";

            switch (sortOrder)
            {
                case "Classroom":
                    items = items.OrderByDescending(s => s.ClassroomId);
                    break;
                case "ClassroomAsc":
                    items = items.OrderBy(s => s.ClassroomId);
                    break;
                case "Class":
                    items = items.OrderByDescending(s => s.ClasssId);
                    break;
                case "ClassAsc":
                    items = items.OrderBy(s => s.ClasssId);
                    break;
                case "Day":
                    items = items.OrderByDescending(s => s.Day);
                    break;
                case "DayAsc":
                    items = items.OrderBy(s => s.Day);
                    break;
                case "Hour":
                    items = items.OrderByDescending(s => s.Hour);
                    break;
                case "HourAsc":
                    items = items.OrderBy(s => s.Hour);
                    break;
                case "Name":
                    items = items.OrderByDescending(s => s.Name);
                    break;
                case "NameAsc":
                    items = items.OrderBy(s => s.Name);
                    break;
                case "Teacher":
                    items = items.OrderByDescending(s => s.TeacherId);
                    break;
                case "TeacherAsc":
                    items = items.OrderBy(s => s.TeacherId);
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

        [Authorize(Roles = "Admins,Teachers")]
        public ActionResult TeacherSubjects(int? page, string teacherId, string sortOrder)
        {
            int currentPage = page ?? 1;
            var items = _subjectRepo.FindByTeacherId(teacherId);

            items = SortItems(sortOrder, items);

            var subjects = items.ToList().Select(a => new TeacherSubjectViewModel
            {
                Id = a.Id,
                Classroom = _classroomRepo.FindById(a.ClassroomId).Name,
                Classs = _classRepo.FindById(a.ClasssId).Name,
                Day = a.Day,
                Hour = a.Hour,
                Name = a.Name,
                Teacher = _teacherRepo.FindById(a.TeacherId).FullName,
                TeacherId = a.TeacherId
            }).ToPagedList(currentPage, 10);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_TeacherSubjectsList", subjects);
            }

            return View(subjects);
        }

        [Authorize(Roles = "Students,Admins,Teachers")]
        public ActionResult StudentSubjects(int? page, string studentId, string sortOrder)
        {
            int currentPage = page ?? 1;
            var items = _subjectRepo.FindByStudentId(studentId);
            items = SortItems(sortOrder, items);

            var subjects = items.ToList().Select(a => new StudentSubjectViewModel
            {
                Id = a.Id,
                Classroom = _classroomRepo.FindById(a.ClassroomId).Name,
                Classs = _classRepo.FindById(a.ClasssId).Name,
                Day = a.Day,
                Hour = a.Hour,
                Name = a.Name,
                Teacher = _teacherRepo.FindById(a.TeacherId).FullName,
                StudentId = studentId,
                Student = _studentRepo.FindById(studentId).FullName
            }).ToPagedList(currentPage, 10);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_StudentSubjectsList", subjects);
            }

            return View(subjects);
        }

        [Authorize(Roles = "Students,Admins,Teachers")]
        public ActionResult ClassSubjects(int? page, string sortOrder, int classId)
        {
            int currentPage = page ?? 1;
            var items = _subjectRepo.FindByClassId(classId);
            items = SortItems(sortOrder, items);

            var subjects = items.ToList().Select(a => new ClassSubjectViewModel
            {
                Id = a.Id,
                Classroom = _classroomRepo.FindById(a.ClassroomId).Name,
                Classs = _classRepo.FindById(a.ClasssId).Name,
                Day = a.Day,
                Hour = a.Hour,
                Name = a.Name,
                Teacher = _teacherRepo.FindById(a.TeacherId).FullName,
                ClassId = classId
            }).ToPagedList(currentPage, 10);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ClassSubjectsList", subjects);
            }

            return View(subjects);
        }

        [Authorize(Roles = "Students,Admins,Teachers")]
        public ActionResult ClassroomSubjects(int? page, string sortOrder, int classroomId)
        {
            int currentPage = page ?? 1;
            var items = _subjectRepo.FindByClassroomId(classroomId);
            items = SortItems(sortOrder, items);

            var subjects = items.ToList().Select(a => new ClassroomSubjectViewModel
            {
                Id = a.Id,
                Classroom = _classroomRepo.FindById(a.ClassroomId).Name,
                Classs = _classRepo.FindById(a.ClasssId).Name,
                Day = a.Day,
                Hour = a.Hour,
                Name = a.Name,
                Teacher = _teacherRepo.FindById(a.TeacherId).FullName,
                ClassroomId = classroomId
            }).ToPagedList(currentPage, 10);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ClassroomSubjectsList", subjects);
            }

            return View(subjects);
        }

        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var subject = _subjectRepo.FindById((int)id);

            if (subject == null)
            {
                return HttpNotFound();
            }
            var subjectVm = new SubjectViewModel
            {
                Id = subject.Id,
                Classroom = _classroomRepo.FindById(subject.ClassroomId).Name,
                Classs = _classRepo.FindById(subject.ClasssId).Name,
                Day = subject.Day,
                Hour = subject.Hour,
                Name = subject.Name,
                Teacher = _teacherRepo.FindById(subject.TeacherId).FullName,
                ClassId = subject.ClasssId
            };

            return View(subjectVm);
        }

        [Authorize(Roles = "Admins")]
        public ActionResult Create()
        {
            if (_teacherRepo.GetAll().ToList().Count == 0)
            {
                if (Request.IsAjaxRequest())
                {
                    ViewBag.Error = ConstantStrings.SubjectCreateNoTeachersError;
                    return PartialView("_CreateError");
                }

                return RedirectToAction("Index", new { error = SubjectCreateError.NoTeachers });
            }
            if (_classRepo.GetAll().ToList().Count == 0)
            {
                if (Request.IsAjaxRequest())
                {
                    ViewBag.Error = ConstantStrings.SubjectCreateNoClassesError;
                    return PartialView("_CreateError");
                }

                return RedirectToAction("Index", new { error = SubjectCreateError.NoClasses });
            }
            if (_classroomRepo.GetAll().ToList().Count == 0)
            {
                if (Request.IsAjaxRequest())
                {
                    ViewBag.Error = ConstantStrings.SubjectCreateNoClassroomsError;
                    return PartialView("_CreateError");
                }
                return RedirectToAction("Index", new { error = SubjectCreateError.NoClassrooms });
            }

            var subjectVm = new SubjectCreateViewModel
            {
                Classes = ConstantStrings.GetClassesSl(),
                Classrooms = ConstantStrings.GetClassroomsSl(),
                Days = ConstantStrings.GetSchoolDaysSl(),
                Hours = ConstantStrings.GetSchoolHoursSl(),
                Teachers = ConstantStrings.GetTeachersSl()
            };

            if (Request.IsAjaxRequest())
                return JavaScript("window.location = '" + Url.Action("Create") + "'");

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
            if (!ModelState.IsValid) return View(subjectVm);

            if (IsClassroomFreeAtDate(subjectVm))
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

                _subjectRepo.Insert(subject);
                _subjectRepo.Save();
                Logs.SaveLog("Create", User.Identity.GetUserId(),
                    "Subject", subject.Id.ToString(), Request.UserHostAddress);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("","Wybrana sala jest zajęta wybranum terminie");

            return View(subjectVm);
        }


        private bool IsClassroomFreeAtDate(SubjectCreateViewModel subjectVm)
        {
            var subjects = _subjectRepo.FindByClassroomAndDate(subjectVm.ClassroomId, 
                                                   (int)subjectVm.Day, subjectVm.Hour).ToList();
            
            return !subjects.Any();
        }

        [Authorize(Roles = "Admins")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = _subjectRepo.FindById((int)id);
            if (subject == null)
            {
                return HttpNotFound();
            }

            var subjectVm = new SubjectCreateViewModel
            {
                Classes = ConstantStrings.GetClassesSl(),
                Classrooms = ConstantStrings.GetClassroomsSl(),
                Days = ConstantStrings.GetSchoolDaysSl(),
                Hours = ConstantStrings.GetSchoolHoursSl(),
                Teachers = ConstantStrings.GetTeachersSl(),
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

                _subjectRepo.Update(subject);
                _subjectRepo.Save();
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
            Subject subject = _subjectRepo.FindById((int)id);
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
            _subjectRepo.Delete(id);
            _subjectRepo.Save();
            Logs.SaveLog("Delete", User.Identity.GetUserId(),
                         "Subject", id.ToString(), Request.UserHostAddress);
            return RedirectToAction("Index");
        }

        public ActionResult Hours(string classroom, string day)
        {         
            var hours = ConstantStrings.GetSchoolHoursSl();

            int cr;
            int  dy;
            if (Int32.TryParse(classroom, out cr) && Int32.TryParse(day, out dy))
            {
                var subjects = _subjectRepo.FindByClassroomAndDay(cr, dy).ToList();
                foreach (var t in subjects)
                {
                    for (int j = hours.Count - 1; j >= 0; j--)
                    {
                        if (t.Hour == Int32.Parse(hours[j].Value))
                        {
                            hours.Remove(hours[j]);
                            break;
                        }
                    }
                }
            }

            return Json(hours, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            _subjectRepo.Dispose();
            base.Dispose(disposing);
        }
    }
}
