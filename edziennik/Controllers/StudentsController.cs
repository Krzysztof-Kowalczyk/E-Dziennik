using System.Linq;
using System.Net;
using System.Web.Mvc;
using edziennik.Models;
using edziennik.Resources;
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
            return View(studentRepo.GetAll());
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
            
            var studentVM = new StudentListItemViewModel()
            {
                ClassName = ConstantStrings.classRepo.FindById(student.ClasssId).Name,
                FirstName = student.FirstName,
                SecondName = student.SecondName,
                Surname = student.Surname,
                Pesel = student.Pesel,
                Marks = markVM
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
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admins")]
        public ActionResult Edit([Bind(Include = "Id,ClassId,Number,FirstName,SecondName,Surname,Pesel")] Student student)
        {
            if (ModelState.IsValid)
            {
                studentRepo.Update(student);
                studentRepo.Save();
                return RedirectToAction("Index");
            }
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

       /* public ActionResult AddMark(string id)
        {         
            var student = repo.FindById(id);
            var studentMark = new StudentAddMark
            {
                FirstName = student.FirstName,
                SecondName = student.SecondName,
                Surname = student.Surname,
                Id = student.Id
            };
            ViewBag.SubjectId = ConstantStrings.getStudentSubjectsSL(student.ClasssId);
            ViewBag.Mark = ConstantStrings.getMarksSL();
            return View(studentMark);
        }
        [HttpPost]
        public ActionResult AddMark(StudentAddMark sam)
        {
            if (ModelState.IsValid)
            {
                var student = repo.FindById(sam.Id);
                student.Marks
                repo.Save();
                return RedirectToAction("Index");
            }
            return View(sam);
        }*/


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
