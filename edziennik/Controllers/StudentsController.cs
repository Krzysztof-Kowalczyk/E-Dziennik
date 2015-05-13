using System.Linq;
using System.Net;
using System.Web.Mvc;
using edziennik.Models;
using edziennik.Resources;
using Microsoft.AspNet.Identity;
using Models.Models;
using Repositories.Repositories;

namespace edziennik.Controllers
{
    [Authorize]
    public class StudentsController : PersonController
    {
        private readonly StudentRepository studentRepo;
        private readonly ClasssRepository classRepo;
        private readonly SubjectRepository subjectRepo;
        private readonly TeacherRepository teacherRepo;

        public StudentsController(StudentRepository _repo, ClasssRepository _classsRepo,
                                  SubjectRepository _subjectRepo, TeacherRepository _teacherRepo)
        {
            studentRepo = _repo;
            classRepo = _classsRepo;
            teacherRepo = _teacherRepo;
            subjectRepo = _subjectRepo;
        }

        // GET: Students
        public ActionResult Index()
        {
            var students = studentRepo.GetAll().Select(a => new StudentListItemViewModel
            {
                FirstName = a.FirstName,
                SecondName = a.SecondName,
                Surname = a.Surname,
                ClassName = classRepo.FindById(a.ClasssId).Name,
                Pesel = a.Pesel,
                Id = a.Id
            });

            return View(students);
        }

        // GET: Students/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = studentRepo.FindById(id);
            if (student == null)
            {
                return HttpNotFound();
            }

            var markVm = student.Marks.Select(m => new MarkViewModel
            {
                Subject = subjectRepo.FindById(m.SubjectId).Name,
                Teacher = teacherRepo.FindById(m.TeacherId).FullName,
                Value = m.Value
            }).ToList();

            var studentVm = new StudentViewModel()
            {
                ClassName = classRepo.FindById(student.ClasssId).Name,
                FirstName = student.FirstName,
                SecondName = student.SecondName,
                Surname = student.Surname,
                Pesel = student.Pesel,
                Marks = markVm,
                Id = student.Id
            };
            return View(studentVm);
        }


        [Authorize(Roles = "Admins")]
        public ActionResult Create()
        {
            var student = new StudentRegisterViewModel
            {
                Classes = ConstantStrings.getClassesSL()
            };

            return View(student);
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admins")]
        public ActionResult Create(StudentRegisterViewModel studentVm)
        {
            if (ModelState.IsValid)
            {
                if (classRepo.FindById(studentVm.ClassId).
                                       Students.Count != ConstantStrings.MaxClassStudentCount)
                {
                    var userid = CreateUser(studentVm, "Students");
                    if (userid == "Error") return View(studentVm);
                    
                    var student = new Student
                    {
                        Id = userid,
                        ClasssId = studentVm.ClassId,
                        FirstName = studentVm.FirstName,
                        SecondName = studentVm.SecondName,
                        Surname = studentVm.Surname,
                        Pesel = studentVm.Login
                    };
                        
                    studentRepo.Insert(student);
                    studentRepo.Save();
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
            Student student = studentRepo.FindById(id);
            if (student == null)
            {
                return HttpNotFound();
            }

            var studentEditVm = new StudentEditViewModel
            {
                FirstName = student.FirstName,
                ClassId = student.ClasssId,
                Email = UserManager.FindById(student.Id).Email,
                Id = student.Id,
                Login = student.Pesel,
                SecondName = student.SecondName,
                Surname = student.Surname,
                Classes = ConstantStrings.getClassesSL()
            };
            
            return View(studentEditVm);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admins")]
        public ActionResult Edit(StudentEditViewModel studentEvm)
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
                    Surname = studentEvm.Surname
                };

                studentRepo.Update(student);
                studentRepo.Save();

                var user = UserManager.FindById(studentEvm.Id);
                user.Email = studentEvm.Email;
                UpdateUser(user, student);
               
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
            Student student = studentRepo.FindById(id);
            if (student == null)
            {
                return HttpNotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admins")]
        public ActionResult DeleteConfirmed(string id)
        {
            studentRepo.Delete(id);
            studentRepo.Save();
            DeleteUser(id);
            return RedirectToAction("Index");
        }

    }
}
