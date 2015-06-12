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
    public class ClasssesController : Controller
    {
        private readonly ClasssRepository _classRepo;
        private readonly TeacherRepository _teacherRepo;

        public ClasssesController(ClasssRepository classRepo, TeacherRepository teacherRepo)
        {
            _classRepo = classRepo;
            _teacherRepo = teacherRepo;
        }

        // GET: Classses
        public ActionResult Index(int? page, int? error, string sortOrder)
        {
            if (error.HasValue && error == 1)
                ViewBag.Error = ConstantStrings.ClassCreateError;

            int currentPage = page ?? 1;
            var items = SortItems(sortOrder);

            var classes = items.ToList().Select(a => new ClassListItemViewModel
            {
                Id = a.Id,
                Name = a.Name,
                Teacher = _teacherRepo.FindById(a.TeacherId).FullName
            });

            var classesPl = classes.ToPagedList(currentPage, 10);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ClassList", classesPl);
            }

            return View(classesPl);

        }


        [NonAction]
        private IQueryable<Classs> SortItems(string sortOrder)
        {
            var items = _classRepo.GetAll();

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
            return items;
        }

        // GET: Classses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classs classs = _classRepo.FindById((int)id);

            if (classs == null)
            {
                return HttpNotFound();
            }

            var classVm = new ClassDetailsViewModel
            {
                Id = classs.Id,
                Name = classs.Name,
                Teacher = _teacherRepo.FindById(classs.TeacherId).FullName,
                TeacherId = classs.TeacherId,
               // Students = classs.Students,
                StudentCount = classs.Students!=null ? classs.Students.Count : 0
            };
            return View(classVm);
        }

        // GET: Classses/Create
        public ActionResult Create()
        {
            if (_teacherRepo.GetAll().ToList().Count == 0)
            {
                if (Request.IsAjaxRequest())
                {
                    ViewBag.Error = ConstantStrings.ClassCreateError;
                    return PartialView("_CreateError");
                }
                return RedirectToAction("Index", new { error = 1 });
            }
            var classVm = new ClassCreateViewModel
            {
                Teachers = ConstantStrings.GetTeachersSl()
            };

            if (Request.IsAjaxRequest())
                return JavaScript("window.location = '" + Url.Action("Create") + "'");
            
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

                _classRepo.Insert(classs);
                _classRepo.Save();
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
            Classs classs = _classRepo.FindById((int)id);
            if (classs == null)
            {
                return HttpNotFound();
            }
            var classVm = new ClassCreateViewModel
            {
                Teachers = ConstantStrings.GetTeachersSl(),
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

                _classRepo.Update(classs);
                _classRepo.Save();
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
            Classs classs = _classRepo.FindById((int)id);
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
            _classRepo.Delete(id);
            _classRepo.Save();
            Logs.SaveLog("Delete", User.Identity.GetUserId(),
                         "Class", id.ToString(), Request.UserHostAddress);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _classRepo.Dispose();
            base.Dispose(disposing);
        }

    }
}
