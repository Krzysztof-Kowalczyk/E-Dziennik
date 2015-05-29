using edziennik.Models;
using edziennik.Resources;
using Microsoft.AspNet.Identity;
using Models.Models;
using Repositories.Repositories;
using System;
using System.Net;
using System.Linq;
using System.Web.Mvc;
using edziennik.Models.ViewModels;
using PagedList;

namespace edziennik.Controllers
{
    [Authorize(Roles = "Admins")]
    public class ClasssesController : Controller
    {
        private readonly ClasssRepository classRepo;
        private readonly TeacherRepository teacherRepo;

        public ClasssesController(ClasssRepository _repo, TeacherRepository _teacherRepo)
        {
            classRepo = _repo;
            teacherRepo = _teacherRepo;
        }

        // GET: Classses
        public ActionResult Index(int? page, int? error, string sortOrder)
        {
            if (error.HasValue)
                ViewBag.Error = ConstantStrings.ClassCreateError;

            int currentPage = page ?? 1;
            var items = classRepo.GetAll();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.IdSort = String.IsNullOrEmpty(sortOrder) ? "IdAsc" : "";
            ViewBag.NameSort = sortOrder == "Name" ? "NameAsc" : "Name";
            ViewBag.TeacherSort = sortOrder == "Teacher" ? "TeacherAsc" : "Teacher";

            switch (sortOrder)
            {
                case "Name":
                    items = items.OrderByDescending(s => s.Name);
                    break;
                case "NameAsc":
                    items = items.OrderBy(s => s.Name);
                    break;
                case "Teacher":
                    items = items.OrderByDescending(s => s.TeacherId);
                    break;
                case "TeacherAsc":
                    items = items.OrderBy(s => s.TeacherId);
                    break;
                case "IdAsc":
                    items = items.OrderBy(s => s.Id);
                    break;
                default:    // id descending
                    items = items.OrderByDescending(s => s.Id);
                    break;
            }

            var classes= items.ToList().Select(a=> new ClassListItemViewModel
            {
                Id = a.Id,
                Name = a.Name,
                Teacher = teacherRepo.FindById(a.TeacherId).FullName
            });

            return View(classes.ToPagedList<ClassListItemViewModel>(currentPage, 10));

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

            var classVm = new ClassDetailsViewModel
            {
                Id = classs.Id,
                Name = classs.Name,
                Teacher = teacherRepo.FindById(classs.TeacherId).FullName,
                Students = classs.Students
            };
            return View(classVm);
        }

        // GET: Classses/Create
        public ActionResult Create()
        {
            if (teacherRepo.GetAll().ToList().Count == 0 )
            {
                return RedirectToAction("Index", 
                      new {error = 1});
            }
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
                Logs.SaveLog("Create", User.Identity.GetUserId(), 
                            "Class", classs.Id.ToString(), Request.UserHostAddress);
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
                Logs.SaveLog("Edit", User.Identity.GetUserId(), 
                             "Class", classs.Id.ToString(), Request.UserHostAddress);
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
            Logs.SaveLog("Delete", User.Identity.GetUserId(),
                         "Class", id.ToString(), Request.UserHostAddress);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            classRepo.Dispose();
            base.Dispose(disposing);
        }

    }
}
