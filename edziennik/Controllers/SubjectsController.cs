using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models.Models;
using Repositories;
using Repositories.Repositories;

namespace edziennik.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly SubjectRepository subjectRepo;
        private readonly TeacherRepository teacherRepo;
        private readonly ClassroomRepository classroomRepo;
        private readonly ClasssRepository classRepo;
       
        public SubjectsController(SubjectRepository sr, TeacherRepository tr, 
                                  ClassroomRepository crr, ClasssRepository  cr)
        {
            subjectRepo = sr;
            teacherRepo = tr;
            classroomRepo = crr;
            classRepo = cr;
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
            Subject subject = subjectRepo.FindById((int) id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        // GET: Subjects/Create
        public ActionResult Create()
        {
            var teachers = teacherRepo.GetAll().Select(c => new SelectListItem
            {
                Value = c.Id,
                Text = c.FirstName + c.Surname

            }).ToList();

            var classrooms = classroomRepo.GetAll().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name

            }).ToList();

            var classes = classRepo.GetAll().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name

            }).ToList();
            
            ViewBag.Teachers = teachers;
            ViewBag.ClassRooms = classrooms;
            ViewBag.Classes = classes;

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
