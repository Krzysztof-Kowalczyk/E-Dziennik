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
        private readonly MarkRepository markRepo;
        private readonly StudentRepository studentRepo;
        private readonly TeacherRepository teacherRepo;
        private readonly SubjectRepository subjectRepo;
        private readonly ClasssRepository classRepo;

        public MarksController(MarkRepository _markRepo, StudentRepository _studentRepo, 
                               TeacherRepository _teacherRepo, SubjectRepository _subjectRepo,
                               ClasssRepository _classRepo)
        {
            markRepo = _markRepo;
            studentRepo = _studentRepo;
            teacherRepo = _teacherRepo;
            subjectRepo = _subjectRepo;
            classRepo = _classRepo;
        }

        // GET: Marks
        public ActionResult Index()
        {
            var marks = markRepo.GetAll().Select(a=> new MarkListItemViewModel
                {
                    Student = studentRepo.FindById(a.StudentId).FullName,
                    Teacher = teacherRepo.FindById(a.TeacherId).FullName,
                    Subject = subjectRepo.FindById(a.SubjectId).Name,
                    Value   = a.Value,
                    Classs  = classRepo.FindByMarkId(a.Id).Name,
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

            var markVm = new MarkDetailsViewModel
            {
                Student = studentRepo.FindById(mark.StudentId).FullName,
                Teacher = teacherRepo.FindById(mark.TeacherId).FullName,
                Subject = subjectRepo.FindById(mark.SubjectId).Name,
                Value = mark.Value,
                Classs = classRepo.FindByMarkId(mark.Id).Name,
                Id = mark.Id,
                TeacherId = mark.TeacherId,
                Description = mark.Description
            };

            return View(markVm);
        }

        // GET: Marks/Create
        public ActionResult Create(string studentId)
        {
            if (studentId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var student = studentRepo.FindById(studentId);
            if (student == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var markVm = new MarkCreateViewModel
            {
                StudentId = studentId,
                TeacherId = User.Identity.GetUserId(),
                Subjects = ConstantStrings.getStudentSubjectsSL(student.ClasssId,User.Identity.GetUserId()),
                Values = ConstantStrings.getMarksSL()
            };

            return View(markVm);
        }

        // POST: Marks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MarkCreateViewModel markVm)
        {
            if (ModelState.IsValid)
            {
                var mark = new Mark
                {
                    Description = markVm.Description,
                    Id = markVm.Id,
                    StudentId = markVm.StudentId,
                    SubjectId = markVm.SubjectId,
                    TeacherId = markVm.TeacherId,
                    Value = markVm.Value
                };

                markRepo.Insert(mark);
                markRepo.Save();
                return RedirectToAction("Index");
            }
           
            return View(markVm);
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
            var student = studentRepo.FindById(mark.StudentId);

            if (student == null)
            {
                return HttpNotFound();
            }

            var markVm = new MarkCreateViewModel
            {
                StudentId = mark.StudentId,
                TeacherId = User.Identity.GetUserId(),
                Subjects = ConstantStrings.getStudentSubjectsSL(student.ClasssId, User.Identity.GetUserId()),
                Values = ConstantStrings.getMarksSL(),
                Description = mark.Description
            };

            return View(markVm);
        }

        // POST: Marks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MarkCreateViewModel markVm)
        {
            if (ModelState.IsValid)
            {
                var mark = new Mark
                {
                    Description = markVm.Description,
                    Id = markVm.Id,
                    StudentId = markVm.StudentId,
                    SubjectId = markVm.SubjectId,
                    TeacherId = markVm.TeacherId,
                    Value = markVm.Value
                };

                markRepo.Update(mark);
                markRepo.Save();
                return RedirectToAction("Index");
            }

            return View(markVm);
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

            var markVm = new MarkDetailsViewModel
            {
                Student = studentRepo.FindById(mark.StudentId).FullName,
                Teacher = teacherRepo.FindById(mark.TeacherId).FullName,
                Subject = subjectRepo.FindById(mark.SubjectId).Name,
                Value = mark.Value,
                Classs = classRepo.FindByMarkId(mark.Id).Name,
                Id = mark.Id,
                TeacherId= mark.TeacherId,
                Description = mark.Description
            };
            
            return View(markVm);
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
    }
}
