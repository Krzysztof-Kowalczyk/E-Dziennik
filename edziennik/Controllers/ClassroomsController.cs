﻿using edziennik.Resources;
using Microsoft.AspNet.Identity;
using Models.Models;
using Repositories.Repositories;
using System.Net;
using System.Web.Mvc;

namespace edziennik.Controllers
{
    [Authorize(Roles = "Admins")]
    public class ClassroomsController : Controller
    {
        private readonly ClassroomRepository classroomRepo;

        public ClassroomsController(ClassroomRepository cr)
        {
            classroomRepo = cr;
        }

        // GET: Classrooms
        public ActionResult Index()
        {
            return View(classroomRepo.GetAll());
        }

        // GET: Classrooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classroom classroom = classroomRepo.FindById((int) id);
            if (classroom == null)
            {
                return HttpNotFound();
            }
            return View(classroom);
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
                classroomRepo.Insert(classroom);
                classroomRepo.Save();
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
            Classroom classroom = classroomRepo.FindById((int) id);
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
                classroomRepo.Update(classroom);
                classroomRepo.Save();
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
            Classroom classroom = classroomRepo.FindById((int) id);
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
            classroomRepo.Delete(id);
            classroomRepo.Save();
            Logs.SaveLog("Edit", User.Identity.GetUserId(), 
                        "Classroom", id.ToString(), Request.UserHostAddress);
            return RedirectToAction("Index");
        }

    }
}
