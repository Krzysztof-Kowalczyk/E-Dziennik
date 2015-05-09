using System.Net;
using System.Web.Mvc;
using edziennik.Resources;
using Models.Models;
using Repositories.Repositories;

namespace edziennik.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly SubjectRepository subjectRepo;
       
        public SubjectsController(SubjectRepository sr)
        {
            subjectRepo = sr;
        }

        // GET: Subjects
        public ActionResult Index()
        {
            return View(subjectRepo.GetAll());
        }

        // GET: Subjects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var subject = subjectRepo.FindById((int) id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        // GET: Subjects/Create
        public ActionResult Create()
        {         
            ViewBag.TeacherId = ConstantStrings.getTeachersSL();
            ViewBag.ClassroomId = ConstantStrings.getClassroomsSL();
            ViewBag.ClasssId = ConstantStrings.getClassesSL();
            ViewBag.Day = ConstantStrings.getSchoolDays();
            ViewBag.Hour = ConstantStrings.getSchoolHours();

            return View();
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Subject subject)
        {
            if (ModelState.IsValid)
            {
                subjectRepo.Insert(subject);
                subjectRepo.Save();
                return RedirectToAction("Index");
            }

            ViewBag.TeacherId = ConstantStrings.getTeachersSL(); ;
            ViewBag.ClassroomId = ConstantStrings.getClassroomsSL();
            ViewBag.ClasssId = ConstantStrings.getClassesSL();
            ViewBag.Day = ConstantStrings.getSchoolDays();
            ViewBag.Hour = ConstantStrings.getSchoolHours();
            return View(subject);
        }

        // GET: Subjects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = subjectRepo.FindById((int) id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,TeacherId,ClassId,ClassroomId")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                subjectRepo.Update(subject);
                subjectRepo.Save();
                return RedirectToAction("Index");
            }
            return View(subject);
        }

        // GET: Subjects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = subjectRepo.FindById((int) id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            subjectRepo.Delete(id);
            subjectRepo.Save();
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
