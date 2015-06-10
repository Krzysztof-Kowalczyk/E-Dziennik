using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using edziennik.Models.ViewModels;
using edziennik.Resources;
using Microsoft.AspNet.Identity;
using Models.Models;
using PagedList;
using Repositories.Repositories;

namespace edziennik.Controllers
{
    [Authorize(Roles = "Admins")]
    public class ClassroomsController : Controller
    {
        private readonly ClassroomRepository _classroomRepo;

        public ClassroomsController(ClassroomRepository classroomRepo)
        {
            _classroomRepo = classroomRepo;
        }

        // GET: Classrooms
        public ActionResult Index(int? page, string sortOrder)
        {
            int currentPage = page ?? 1;
            var items = SortItems(sortOrder);
            var itemsPl = items.ToPagedList(currentPage, 10);

               if(Request.IsAjaxRequest())
               {
                   return PartialView("_ClassroomList",itemsPl);
               }

            return View(itemsPl);
        }

        [NonAction]
        private IQueryable<Classroom> SortItems(string sortOrder)
        {
            var items = _classroomRepo.GetAll();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.IdSort = String.IsNullOrEmpty(sortOrder) ? "IdAsc" : "";
            ViewBag.NameSort = sortOrder == "NameAsc" ? "Name" : "NameAsc";

            switch (sortOrder)
            {
                case "Name":
                    items = items.OrderByDescending(s => s.Name);
                    break;
                case "NameAsc":
                    items = items.OrderBy(s => s.Name);
                    break;
                case "IdAsc":
                    items = items.OrderBy(s => s.Id);
                    break;
                default:    // id descending
                    items = items.OrderByDescending(s => s.Id);
                    break;
            }
            return items;
        }

        // GET: Classrooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classroom classroom = _classroomRepo.FindById((int)id);
            if (classroom == null)
            {
                return HttpNotFound();
            }
            var classroomVm = new ClassroomDetailsViewModel
            {
                Id = classroom.Id,
                Name = classroom.Name,
                SubjectsCount = classroom.Subjects != null ? classroom.Subjects.Count : 0
            };
            return View(classroomVm);
        }

        // GET: Classrooms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Classrooms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Classroom classroom)
        {
            if (ModelState.IsValid)
            {
                _classroomRepo.Insert(classroom);
                _classroomRepo.Save();
                Logs.SaveLog("Create", User.Identity.GetUserId(),
                            "Classroom", classroom.Id.ToString(), Request.UserHostAddress);
                return RedirectToAction("Index");
            }

            return View(classroom);
        }

        // GET: Classrooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classroom classroom = _classroomRepo.FindById((int)id);
            if (classroom == null)
            {
                return HttpNotFound();
            }
            return View(classroom);
        }

        // POST: Classrooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Classroom classroom)
        {
            if (ModelState.IsValid)
            {
                _classroomRepo.Update(classroom);
                _classroomRepo.Save();
                Logs.SaveLog("Edit", User.Identity.GetUserId(),
                             "Classroom", classroom.Id.ToString(), Request.UserHostAddress);
                return RedirectToAction("Index");
            }
            return View(classroom);
        }

        // GET: Classrooms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classroom classroom = _classroomRepo.FindById((int)id);
            if (classroom == null)
            {
                return HttpNotFound();
            }
            return View(classroom);
        }

        // POST: Classrooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _classroomRepo.Delete(id);
            _classroomRepo.Save();
            Logs.SaveLog("Edit", User.Identity.GetUserId(),
                        "Classroom", id.ToString(), Request.UserHostAddress);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _classroomRepo.Dispose();
            base.Dispose(disposing);
        }

    }
}
