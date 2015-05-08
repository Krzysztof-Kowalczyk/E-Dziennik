using System.Linq;
using System.Net;
using System.Web.Mvc;
using edziennik.Models;
using Models.Models;
using Repositories.Repositories;

namespace edziennik.Controllers
{
    public class StudentsController : PersonController
    {
        private readonly StudentRepository repo;

        private readonly ClasssRepository crepo;

        public StudentsController(StudentRepository _repo, ClasssRepository _crepo)
        { 
            repo = _repo;
            crepo = _crepo;
        }

        // GET: Students
        public ActionResult Index()
        {
            return View(repo.GetAll());
        }

        // GET: Students/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = repo.FindById(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }


        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.ClassId= new SelectList(crepo.GetAll(), "Id", "Name");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StudentRegisterViewModel studentVM)
        {
            if (ModelState.IsValid)
            {
                var userid = CreateUser(studentVM,"Students");
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
                    repo.Insert(student);
                    repo.Save();
                    return RedirectToAction("Index");
                }
            }

           ViewBag.ClassId = new SelectList(crepo.GetAll(), "Id", "Name");

           return View(studentVM);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = repo.FindById(id);
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
        public ActionResult Edit([Bind(Include = "Id,ClassId,Number,FirstName,SecondName,Surname,Pesel")] Student student)
        {
            if (ModelState.IsValid)
            {
                repo.Update(student);
                repo.Save();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = repo.FindById(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
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
                db.Dispose();
            }
            base.Dispose(disposing);
        }*/
    }
}
