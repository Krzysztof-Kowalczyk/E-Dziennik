using System.Net;
using System.Web.Mvc;
using edziennik.Models;
using edziennik.Resources;
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
                Logs.SaveLog("Create", User.Identity.GetUserId(), "Teacher", teacher.Id);
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

            var teacherEditVm = new TeacherEditViewModel
            {
                FirstName = teacher.FirstName,
                Email = UserManager.FindById(teacher.Id).Email,
                Id = teacher.Id,
                Login = teacher.Pesel,
                SecondName = teacher.SecondName,
                Surname = teacher.Surname,
            };

            return View(teacherEditVm);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TeacherEditViewModel teacherVm)
        {
            if (ModelState.IsValid)
            {
                var teacher = new Teacher
                {
                    Id = teacherVm.Id,
                    FirstName = teacherVm.FirstName,
                    Pesel = teacherVm.Login,
                    SecondName = teacherVm.SecondName,
                    Surname = teacherVm.Surname
                };

                teacherRepo.Update(teacher);
                teacherRepo.Save();
                
                var user = UserManager.FindById(teacherVm.Id);
                user.Email = teacherVm.Email;
                UpdateUser(user,teacher);
                Logs.SaveLog("Edit", User.Identity.GetUserId(), "Teacher", teacher.Id);

                return RedirectToAction("Index");
            }
            return View(teacherVm);
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
            Logs.SaveLog("Delete", User.Identity.GetUserId(), "Teacher", id);
            return RedirectToAction("Index");
        }

    }
}
