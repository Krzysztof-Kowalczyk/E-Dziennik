using System.Net;
using System.Web.Mvc;
using edziennik.Resources;
using Models.Models;
using Repositories.Repositories;

namespace edziennik.Controllers
{
    [Authorize(Roles = "Admins")]
    public class ClasssesController : Controller
    {
        private readonly ClasssRepository repo;

        public ClasssesController(ClasssRepository _repo)
        {
            repo = _repo;
        }

        // GET: Classses
        public ActionResult Index()
        {
            return View(repo.GetAll());
        }

        // GET: Classses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classs classs = repo.FindById((int)id);

            if (classs == null)
            {
                return HttpNotFound();
            }
            return View(classs);
        }

        // GET: Classses/Create
        public ActionResult Create()
        {
            ViewBag.Teachers = ConstantStrings.getTeachersSL();
            return View();
        }

        // POST: Classses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Classs classs)
        {
            if (ModelState.IsValid)
            {
                repo.Insert(classs);
                repo.Save();
                return RedirectToAction("Index");
            }
            ViewBag.Teachers = ConstantStrings.getTeachersSL();
            return View(classs);
        }

        // GET: Classses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classs classs = repo.FindById((int)id);
            if (classs == null)
            {
                return HttpNotFound();
            }
            return View(classs);
        }

        // POST: Classses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Classs classs)
        {
            if (ModelState.IsValid)
            {
                repo.Update(classs);
                repo.Save();
                return RedirectToAction("Index");
            }
            return View(classs);
        }

        // GET: Classses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classs classs = repo.FindById((int) id);
            if (classs == null)
            {
                return HttpNotFound();
            }
            return View(classs);
        }

        // POST: Classses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            repo.Delete(id);
            repo.Save();
            return RedirectToAction("Index");
        }

      /*  protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }*/
    }
}
