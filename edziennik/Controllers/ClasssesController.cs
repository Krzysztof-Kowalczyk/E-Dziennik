using System.Net;
using System.Web.Mvc;
using edziennik.Models;
using edziennik.Resources;
using Models.Models;
using Repositories.Repositories;

namespace edziennik.Controllers
{
    [Authorize(Roles = "Admins")]
    public class ClasssesController : Controller
    {
        private readonly ClasssRepository classRepo;

        public ClasssesController(ClasssRepository _repo)
        {
            classRepo = _repo;
        }

        // GET: Classses
        public ActionResult Index()
        {
            return View(classRepo.GetAll());
        }

        // GET: Classses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classs classs = classRepo.FindById((int)id);

            if (classs == null)
            {
                return HttpNotFound();
            }
            return View(classs);
        }

        // GET: Classses/Create
        public ActionResult Create()
        {
            var classVm = new ClassCreateViewModel
            {
                Teachers = ConstantStrings.getTeachersSL()
            };

            return View(classVm);
        }

        // POST: Classses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClassCreateViewModel classVm)
        {
            if (ModelState.IsValid)
            {
                var classs = new Classs()
                {
                    Id = classVm.Id,
                    Name = classVm.Name,
                    TeacherId = classVm.TeacherId
                };

                classRepo.Insert(classs);
                classRepo.Save();
                return RedirectToAction("Index");
            }

            return View(classVm);
        }

        // GET: Classses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classs classs = classRepo.FindById((int)id);
            if (classs == null)
            {
                return HttpNotFound();
            }
            var classVm = new ClassCreateViewModel
            {
                Teachers = ConstantStrings.getTeachersSL(),
                Id = classs.Id,
                Name = classs.Name,
                TeacherId = classs.TeacherId
            };

            return View(classVm);
        }

        // POST: Classses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,TeacherId")] ClassCreateViewModel classVm)
        {
            if (ModelState.IsValid)
            {
                var classs = new Classs()
                {
                    Id = classVm.Id,
                    Name = classVm.Name,
                    TeacherId = classVm.TeacherId
                };

                classRepo.Update(classs);
                classRepo.Save();
                return RedirectToAction("Index");
            }
            return View(classVm);
        }

        // GET: Classses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classs classs = classRepo.FindById((int) id);
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
            classRepo.Delete(id);
            classRepo.Save();
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
