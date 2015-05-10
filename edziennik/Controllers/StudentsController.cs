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

        public StudentsController(StudentRepository _repo)
        {
            studentRepo = _repo;
        }

        // GET: Students
        public ActionResult Index()
        {
            var students = studentRepo.GetAll().Select(a => new StudentListItemViewModel
            {
                FirstName = a.FirstName,
                SecondName = a.SecondName,
                Surname = a.Surname,
                ClassName = ConstantStrings.classRepo.FindById(a.ClasssId).Name,
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

            var markVM = student.Marks.Select(m => new MarkViewModel
            {
                Subject = ConstantStrings.subjectRepo.FindById(m.SubjectId).Name,
                Teacher = ConstantStrings.teacherRepo.FindById(m.TeacherId).FullName,
                Value = m.Value
            }).ToList();

            var studentVM = new StudentViewModel()
            {
                ClassName = ConstantStrings.classRepo.FindById(student.ClasssId).Name,
                FirstName = student.FirstName,
                SecondName = student.SecondName,
                Surname = student.Surname,
                Pesel = student.Pesel,
                Marks = markVM,
                Id = student.Id
            };
            return View(studentVM);
        }


        [Authorize(Roles = "Admins")]
        public ActionResult Create()
        {
            ViewBag.ClassId = ConstantStrings.getClassesSL();
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admins")]
        public ActionResult Create(StudentRegisterViewModel studentVM)
        {
            if (ModelState.IsValid)
            {
                if (ConstantStrings.getClassStudentCount(studentVM.ClassId) != 30)
                {
                    var userid = CreateUser(studentVM, "Students");
                    if (userid != "Error")
                    {
                        var student = new Student()
                        {
                            Id = userid,
                            ClasssId = studentVM.ClassId,
                            FirstName = studentVM.FirstName,
                            SecondName = studentVM.SecondName,
                            Surname = studentVM.Surname,
                            Pesel = studentVM.Login
                        };
                        studentRepo.Insert(student);
                        studentRepo.Save();
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Ta klasa posiada już maksymalną ilość uczniów !");
                }
            }

            ViewBag.ClassId = ConstantStrings.getClassesSL();

            return View(studentVM);
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

            var studentEditVM = new StudentEditViewModel
            {
                FirstName = student.FirstName,
                ClassId = student.ClasssId,
                Email = UserManager.FindById(student.Id).Email,
                Id = student.Id,
                Login = student.Pesel,
                SecondName = student.SecondName,
                Surname = student.Surname
            };
            ViewBag.ClassId = ConstantStrings.getClassesSL();

            return View(studentEditVM);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admins")]
        public ActionResult Edit(StudentEditViewModel student)
        {
            if (ModelState.IsValid)
            {
                UpdateFromEditVM(student);
                var user = UserManager.FindById(student.Id);
                user.Email = student.Email;
                user.UserName = student.Login;
                ApplicationDbContext.Create().SaveChanges();
                
                return RedirectToAction("Index");
            }
            ViewBag.ClassId = ConstantStrings.getClassesSL();

            return View(student);
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

            var studentVM = new StudentListItemViewModel()
            {
                ClassName = ConstantStrings.classRepo.FindById(student.ClasssId).Name,
                FirstName = student.FirstName,
                SecondName = student.SecondName,
                Surname = student.Surname,
                Pesel = student.Pesel
            };

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

        private void UpdateFromEditVM(StudentEditViewModel studentEVM)
        {
            var studentToUpdate = studentRepo.FindById(studentEVM.Id);
            studentToUpdate.ClasssId = studentEVM.ClassId;
            studentToUpdate.FirstName = studentEVM.FirstName;
            studentToUpdate.Pesel = studentEVM.Login;
            studentToUpdate.SecondName = studentEVM.SecondName;
            studentToUpdate.Surname = studentEVM.Surname;
            studentRepo.Save();
        }

        /* protected override void Dispose(bool disposing)
         {
             if (disposing)
             {
                 db.Dispose();
             }
             base.Dispose(disposing);
         }*/
    }
}
