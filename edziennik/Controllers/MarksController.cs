using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using edziennik.Resources;
using Microsoft.AspNet.Identity;
using Models.Models;
using Repositories;
using Repositories.Repositories;
using edziennik.Models;
using edziennik.Validators;

namespace edziennik.Controllers
{
    [Authorize(Roles = "Teachers, Admins")]
    public class MarksController : Controller
    {
        private readonly MarkRepository markRepo = new MarkRepository();

        // GET: Marks
        public ActionResult Index()
        {
            var marks = markRepo.GetAll().Select(a=> new MarkListItemViewModel
                {
                    Student = ConstantStrings.studentRepo.FindById(a.StudentId).FullName,
                    Teacher = ConstantStrings.teacherRepo.FindById(a.TeacherId).FullName,
                    Subject = ConstantStrings.subjectRepo.FindById(a.SubjectId).Name,
                    Value   = a.Value,
                    Classs  = ConstantStrings.classRepo.FindByStudent(ConstantStrings.studentRepo.FindByMark(a.Id).Id).Name,
                    Id = a.Id,
                    TeacherId = a.TeacherId
                    
                });
           
            return View(marks);
        }

        // GET: Marks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mark mark = markRepo.FindById((int)id);
            if (mark == null)
            {
                return HttpNotFound();
            }

            var markVM = new MarkDetailsViewModel
            {
                Student = ConstantStrings.studentRepo.FindById(mark.StudentId).FullName,
                Teacher = ConstantStrings.teacherRepo.FindById(mark.TeacherId).FullName,
                Subject = ConstantStrings.subjectRepo.FindById(mark.SubjectId).Name,
                Value = mark.Value,
                Classs = ConstantStrings.classRepo.FindByStudent(ConstantStrings.studentRepo.FindByMark(mark.Id).Id).Name,
                Id = mark.Id,
                TeacherId = mark.TeacherId,
                Description = mark.Description
            };

            return View(markVM);
        }

        // GET: Marks/Create
        public ActionResult Create(string studentId)
        {
            if (studentId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var student = ConstantStrings.studentRepo.FindById(studentId);
            if (student == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var mark = new Mark
            {
                StudentId = studentId,
                TeacherId = User.Identity.GetUserId()
            };

            ViewBag.SubjectId = ConstantStrings.getStudentSubjectsSL
                                                  (student.ClasssId,User.Identity.GetUserId());
            ViewBag.Value = ConstantStrings.getMarksSL();
            return View(mark);
        }

        // POST: Marks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Mark mark)
        {
            if (ModelState.IsValid)
            {
                markRepo.Insert(mark);
                markRepo.Save();
                return RedirectToAction("Index");
            }
            
            var student = ConstantStrings.studentRepo.FindById(mark.StudentId);
            ViewBag.SubjectId = ConstantStrings.getStudentSubjectsSL
                                                  (student.ClasssId, User.Identity.GetUserId());
            ViewBag.Value = ConstantStrings.getMarksSL();

            return View(mark);
        }

        // GET: Marks/Edit/5
        [OnlyMarkTeacherOrAdmin]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mark mark = markRepo.FindById((int)id);
            if (mark == null)
            {
                return HttpNotFound();
            }
            var student = ConstantStrings.studentRepo.FindById(mark.StudentId);
            ViewBag.SubjectId = ConstantStrings.getStudentSubjectsSL
                                                  (student.ClasssId, User.Identity.GetUserId());
            ViewBag.Value = ConstantStrings.getMarksSL();
            return View(mark);
        }

        // POST: Marks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Mark mark)
        {
            if (ModelState.IsValid)
            {
                markRepo.Update(mark);
                markRepo.Save();
                return RedirectToAction("Index");
            }
            var student = ConstantStrings.studentRepo.FindById(mark.StudentId);
            ViewBag.SubjectId = ConstantStrings.getStudentSubjectsSL
                                                  (student.ClasssId, User.Identity.GetUserId());
            ViewBag.Value = ConstantStrings.getMarksSL();
            return View(mark);
        }

        // GET: Marks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mark mark = markRepo.FindById((int)id);
            if (mark == null)
            {
                return HttpNotFound();
            }

            var markVM = new MarkDetailsViewModel
            {
                Student = ConstantStrings.studentRepo.FindById(mark.StudentId).FullName,
                Teacher = ConstantStrings.teacherRepo.FindById(mark.TeacherId).FullName,
                Subject = ConstantStrings.subjectRepo.FindById(mark.SubjectId).Name,
                Value = mark.Value,
                Classs = ConstantStrings.classRepo.FindByStudent(ConstantStrings.studentRepo.FindByMark(mark.Id).Id).Name,
                Id = mark.Id,
                TeacherId= mark.TeacherId,
                Description = mark.Description
            };
            
            return View(markVM);
        }

        // POST: Marks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            markRepo.Delete(id);
            markRepo.Save();
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        markRepo.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
