using System.Net;
using System.Web.Mvc;
using edziennik.Models;
using Models.Models;
using Repositories.Repositories;

namespace edziennik.Controllers
{
    [Authorize(Roles = "Admins")]
    public class TeachersController : PersonController
    {
        private readonly TeacherRepository repo;

        public TeachersController(TeacherRepository _repo)
        {
            repo = _repo;
        }

        // GET: Teachers
        public ActionResult Index()
        {
            return View(repo.GetAll());
        }

        // GET: Teachers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = repo.FindById(id);
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
        public ActionResult Create(TeacherRegisterViewModel teacherVM)
        {
            if (ModelState.IsValid)
            {
                var userid = CreateUser(teacherVM, "Teachers");
                if (userid != "Error")
                {
                    var teacher = new Teacher()
                    {
                        Id = userid,
                        FirstName = teacherVM.FirstName,
                        SecondName = teacherVM.SecondName,
                        Surname = teacherVM.Surname,
                        Pesel = teacherVM.Login
                    };
                    repo.Insert(teacher);
                    repo.Save();
                    return RedirectToAction("Index");
                }
            }

            return View(teacherVM);
        }

        // GET: Teachers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = repo.FindById(id);
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
        public ActionResult Edit([Bind(Include = "Id,FirstName,SecondName,Surname,Pesel")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                repo.Update(teacher);
                repo.Save();
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
            Teacher teacher = repo.FindById(id);
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
            repo.Delete(id);
            repo.Save();
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
