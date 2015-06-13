using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using edziennik.Models.ViewModels;
using edziennik.Resources;
using Microsoft.AspNet.Identity;
using Models.Models;
using PagedList;
using Repositories.Repositories;

namespace edziennik.Controllers
{
    [Authorize]
    public class StudentsController : PersonController
    {
        private readonly StudentRepository _studentRepo;
        private readonly ClasssRepository _classRepo;
        private readonly SubjectRepository _subjectRepo;
        private readonly TeacherRepository _teacherRepo;

        public StudentsController(ApplicationUserManager userManager,StudentRepository studentRepo, ClasssRepository classsRepo,
                                  SubjectRepository subjectRepo, TeacherRepository teacherRepo)
            :base(userManager)
        {
            _studentRepo = studentRepo;
            _classRepo = classsRepo;
            _teacherRepo = teacherRepo;
            _subjectRepo = subjectRepo;
        }

        // GET: Students
        public ActionResult Index(int? page, int? error, string sortOrder)
        {
            if (error.HasValue)
                ViewBag.Error = ConstantStrings.StudentCreateNoClassesError;

            int currentPage = page ?? 1;
            
            var items = _studentRepo.GetAll();
            items = SortItems(sortOrder, items);

            var students = items.ToList().Select(a => new StudentListItemViewModel
            {
                FirstName = a.FirstName,
                SecondName = a.SecondName,
                Surname = a.Surname,
                ClassName = _classRepo.FindById(a.ClasssId).Name,
                Pesel = a.Pesel,
                Id = a.Id
            }).ToPagedList(currentPage, 10);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_StudentList", students);
            }

            return View(students);
        }

        public ActionResult ClassStudents(int? page, int? error, string sortOrder, int? classId)
        {
            if (classId == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (error.HasValue)
                ViewBag.Error = ConstantStrings.StudentCreateNoClassesError;

            int currentPage = page ?? 1;

            var items = _studentRepo.FindByClassId((int)classId);
            items = SortItems(sortOrder, items);

            var students = items.ToList().Select(a => new ClassStudentViewModel
            {
                FirstName = a.FirstName,
                SecondName = a.SecondName,
                Surname = a.Surname,
                ClassName = _classRepo.FindById(a.ClasssId).Name,
                Pesel = a.Pesel,
                Id = a.Id,
                ClassId = a.ClasssId
            }).ToPagedList(currentPage, 10);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ClassStudentList", students);
            }

            return View(students);
        }

        public ActionResult TeacherClassStudents(int? page, int? error, string sortOrder, string teacherId)
        {
            if (teacherId == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (error.HasValue)
                ViewBag.Error = ConstantStrings.StudentCreateNoClassesError;

            int currentPage = page ?? 1;

            var items = _studentRepo.FindByTutorId(teacherId);

            items = SortItems(sortOrder, items);

            var students = items.ToList().Select(a => new TeacherClassStudentViewModel
            {
                FirstName = a.FirstName,
                SecondName = a.SecondName,
                Surname = a.Surname,
                ClassName = _classRepo.FindById(a.ClasssId).Name,
                Pesel = a.Pesel,
                Id = a.Id,
                TeacherId = teacherId
            }).ToPagedList(currentPage, 10);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ClassStudentList", students);
            }

            return View(students);
        }

        public ActionResult SubjectStudents(int? page, int? error, string sortOrder, int? subjectId)
        {
            if (subjectId == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (error.HasValue)
                ViewBag.Error = ConstantStrings.StudentCreateNoClassesError;

            int currentPage = page ?? 1;

            var items = _studentRepo.FindBySubjectId((int)subjectId);
            items = SortItems(sortOrder, items);

            var students = items.ToList().Select(a => new SubjectStudentViewModel
            {
                FirstName = a.FirstName,
                SecondName = a.SecondName,
                Surname = a.Surname,
                ClassName = _classRepo.FindById(a.ClasssId).Name,
                Pesel = a.Pesel,
                Id = a.Id,
                SubjectId = (int)subjectId,
                SubjectName = _subjectRepo.FindById((int)subjectId).Name

            }).ToPagedList(currentPage, 10);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_StudentStudentList", students);
            }

            return View(students);
        }

        [NonAction]
        private IQueryable<Student> SortItems(string sortOrder, IQueryable<Student> items)
        {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.IdSort = String.IsNullOrEmpty(sortOrder) ? "IdAsc" : "";
            ViewBag.ClassSort = sortOrder == "ClassAsc" ? "Class" : "ClassAsc";
            ViewBag.FirstNameSort = sortOrder == "FirstNameAsc" ? "FirstName" : "FirstNameAsc";
            ViewBag.SecondNameSort = sortOrder == "SecondNameAsc" ? "SecondName" : "SecondNameAsc";
            ViewBag.SurnameSort = sortOrder == "SurnameAsc" ? "Surname" : "SurnameAsc";

            switch (sortOrder)
            {
                case "Class":
                    items = items.OrderByDescending(s => s.ClasssId);
                    break;
                case "ClassAsc":
                    items = items.OrderBy(s => s.ClasssId);
                    break;
                case "FirstName":
                    items = items.OrderByDescending(s => s.FirstName);
                    break;
                case "FirstNameAsc":
                    items = items.OrderBy(s => s.FirstName);
                    break;
                case "SecondName":
                    items = items.OrderByDescending(s => s.SecondName);
                    break;
                case "SecondNameAsc":
                    items = items.OrderBy(s => s.SecondName);
                    break;
                case "Surname":
                    items = items.OrderByDescending(s => s.Surname);
                    break;
                case "SurnameAsc":
                    items = items.OrderBy(s => s.Surname);
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

        // GET: Students/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = _studentRepo.FindById(id);
            if (student == null)
            {
                return HttpNotFound();
            }

            var markVm = student.Marks.Select(m => new MarkViewModel
            {
                Subject = _subjectRepo.FindById(m.SubjectId).Name,
                Teacher = _teacherRepo.FindById(m.TeacherId).FullName,
                Value = m.Value
            }).ToList();

            var user = UserManager.FindById(student.Id);
            
            var studentVm = new StudentDetailsViewModel()
            {
                ClassName = _classRepo.FindById(student.ClasssId).Name,
                FirstName = student.FirstName,
                SecondName = student.SecondName,
                Surname = student.Surname,
                Pesel = student.Pesel,
                Id = student.Id,
                Marks = markVm,
                CellPhoneNumber = student.CellPhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
                AvatarUrl = user.AvatarUrl,
            };
            return View(studentVm);
        }


        [Authorize(Roles = "Admins")]
        public ActionResult Create()
        {
            if (_classRepo.GetAll().ToList().Count == 0)
            {
                return RedirectToAction("Index", new { error = 1 });
            }

            var student = new StudentRegisterViewModel
            {
                Classes = ConstantStrings.GetClassesSl()
            };

            return View(student);
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admins")]
        public async Task<ActionResult> Create(StudentRegisterViewModel studentVm)
        {
            if (ModelState.IsValid)
            {
                if (_classRepo.FindById(studentVm.ClassId).
                                       Students.Count != ConstantStrings.MaxClassStudentCount)
                {
                    var userid = await CreateUser(studentVm, "Students");
                    if (userid == "Error") return View(studentVm);
                    
                    var student = new Student
                    {
                        Id = userid,
                        ClasssId = studentVm.ClassId,
                        FirstName = studentVm.FirstName,
                        SecondName = studentVm.SecondName,
                        Surname = studentVm.Surname,
                        Pesel = studentVm.Login,
                        CellPhoneNumber = studentVm.CellPhoneNumber

                    };
                        
                    _studentRepo.Insert(student);
                    _studentRepo.Save();
                    Logs.SaveLog("Create", User.Identity.GetUserId(), 
                                 "Student", student.Id, Request.UserHostAddress);
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Ta klasa posiada już maksymalną ilość uczniów !");
            }

            return View(studentVm);
        }

        [Authorize(Roles = "Admins")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = _studentRepo.FindById(id);
            if (student == null)
            {
                return HttpNotFound();
            }

            var user = UserManager.FindById(student.Id);
            var studentEditVm = new StudentEditViewModel
            {
                FirstName = student.FirstName,
                ClassId = student.ClasssId,
                Email = user.Email,
                Id = student.Id,
                Login = student.Pesel,
                SecondName = student.SecondName,
                Surname = student.Surname,
                Classes = ConstantStrings.GetClassesSl(),
                CellPhoneNumber = student.CellPhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
                AvatarUrl = user.AvatarUrl
            };
            
            return View(studentEditVm);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admins")]
        public async Task<ActionResult> Edit(StudentEditViewModel studentEvm)
        {
            if (ModelState.IsValid)
            {
                var student = new Student
                {
                    ClasssId = studentEvm.ClassId,
                    FirstName = studentEvm.FirstName,
                    Id = studentEvm.Id,
                    Pesel = studentEvm.Login,
                    SecondName = studentEvm.SecondName,
                    Surname = studentEvm.Surname,
                    CellPhoneNumber = studentEvm.CellPhoneNumber
                };

                _studentRepo.Update(student);
                _studentRepo.Save();               

                var user = await UserManager.FindByIdAsync(studentEvm.Id);                
                await UpdateUser(user, student, studentEvm.Email,studentEvm.EmailConfirmed);
                Logs.SaveLog("Edit", User.Identity.GetUserId(), 
                             "Student", student.Id, Request.UserHostAddress);
               
                return RedirectToAction("Index");
            }

            return View(studentEvm);
        }

        // GET: Students/Delete/5
        [Authorize(Roles = "Admins")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = _studentRepo.FindById(id);
            if (student == null)
            {
                return HttpNotFound();
            }

            var studentVm = new StudentListItemViewModel
            {
                ClassName = _classRepo.FindById(student.ClasssId).Name,
                FirstName = student.FirstName,
                Id = student.Id,
                Pesel = student.Pesel,
                SecondName = student.SecondName,
                Surname = student.Surname
            };

            return View(studentVm);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admins")]
        public ActionResult DeleteConfirmed(string id)
        {
            _studentRepo.Delete(id);
            _studentRepo.Save();
            DeleteUser(id);
            Logs.SaveLog("Delete", User.Identity.GetUserId(),
                         "Student", id, Request.UserHostAddress);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _studentRepo.Dispose();
            base.Dispose(disposing);
        }

    }
}
