using System.Net;
using System.Web.Mvc;
using edziennik.Models;
using Microsoft.AspNet.Identity;
using Models.Models;
using Repositories.Repositories;

namespace edziennik.Controllers
{
    [Authorize(Roles = "Admins")]
    public class TeachersController : PersonController
    {
        private readonly TeacherRepository teacherRepo;

        public TeachersController(TeacherRepository _repo)
        {
            teacherRepo = _repo;
        }

        // GET: Teachers
        public ActionResult Index()
        {
            return View(teacherRepo.GetAll());
        }

        // GET: Teachers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = teacherRepo.FindById(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // GET: Teachers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TeacherRegisterViewModel teacherVm)
        {
            if (ModelState.IsValid)
            {
                var userid = CreateUser(teacherVm, "Teachers");
                
                if (userid == "Error") return View(teacherVm);
                
                var teacher = new Teacher()
                {
                    Id = userid,
                    FirstName = teacherVm.FirstName,
                    SecondName = teacherVm.SecondName,
                    Surname = teacherVm.Surname,
                    Pesel = teacherVm.Login
                };
                teacherRepo.Insert(teacher);
                teacherRepo.Save();
                return RedirectToAction("Index");
            }

            return View(teacherVm);
        }

        // GET: Teachers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = teacherRepo.FindById(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                teacherRepo.Update(teacher);
                teacherRepo.Save();
                var user = UserManager.FindById(teacher.Id);
                user.UserName = teacher.Pesel;
                UpdateUser(user,teacher);
                
                return RedirectToAction("Index");
            }
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = teacherRepo.FindById(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            teacherRepo.Delete(id);
            teacherRepo.Save();
            DeleteUser(id);
            return RedirectToAction("Index");
        }

       /* protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repo.Dispose();
            }
            base.Dispose(disposing);
        }*/
    }
}
