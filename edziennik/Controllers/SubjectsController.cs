using System.Linq;
using System.Net;
using System.Web.Mvc;
using edziennik.Models;
using edziennik.Resources;
using Models.Models;
using Repositories.Repositories;

namespace edziennik.Controllers
{
    [Authorize(Roles = "Admins")]
    public class SubjectsController : Controller
    {
        private readonly SubjectRepository subjectRepo;
        private readonly ClasssRepository classRepo;
        private readonly ClassroomRepository classroomRepo;
        private readonly TeacherRepository teacherRepo;

        public SubjectsController(SubjectRepository sr, ClasssRepository _classsRepo,
                                  ClassroomRepository _classroomRepo, TeacherRepository _teacherRepo)
        {
            subjectRepo = sr;
            classRepo = _classsRepo;
            classroomRepo = _classroomRepo;
            teacherRepo = _teacherRepo;
        }

        // GET: Subjects
        public ActionResult Index()
        {
            var subjects = subjectRepo.GetAll().Select(a => new SubjectViewModel
            {
                Id = a.Id,
                Classroom = classroomRepo.FindById(a.ClassroomId).Name,
                Classs = classRepo.FindById(a.ClasssId).Name,
                Day = a.Day,
                Hour = a.Hour,
                Name = a.Name,
                Teacher = teacherRepo.FindById(a.TeacherId).FullName
            });
            
            return View(subjects);
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
            var subjectVm = new SubjectViewModel
            {
                Id = subject.Id,
                Classroom = classroomRepo.FindById(subject.ClassroomId).Name,
                Classs = classRepo.FindById(subject.ClasssId).Name,
                Day = subject.Day,
                Hour = subject.Hour,
                Name = subject.Name,
                Teacher = teacherRepo.FindById(subject.TeacherId).FullName
            };

            return View(subjectVm);
        }

        // GET: Subjects/Create
        public ActionResult Create()
        {
            var subjectVm = new SubjectCreateViewModel
            {
                Classes = ConstantStrings.getClassesSL(),
                Classrooms = ConstantStrings.getClassroomsSL(),
                Days = ConstantStrings.getSchoolDaysSL(),
                Hours = ConstantStrings.getSchoolHoursSL(),
                Teachers = ConstantStrings.getTeachersSL()
            };

            return View(subjectVm);
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SubjectCreateViewModel subjectVm)
        {
            if (!ModelState.IsValid)
            {
                var subject = new Subject
                {
                    ClassroomId = subjectVm.ClassroomId,
                    ClasssId = subjectVm.ClasssId,
                    Day = subjectVm.Day,
                    Hour = subjectVm.Hour,
                    Id = subjectVm.Id,
                    Name = subjectVm.Name,
                    TeacherId = subjectVm.TeacherId
                };

                subjectRepo.Insert(subject);
                subjectRepo.Save();
                return RedirectToAction("Index");
            }

            return View(subjectVm);
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

            var subjectVm = new SubjectCreateViewModel
            {
                Classes = ConstantStrings.getClassesSL(),
                Classrooms = ConstantStrings.getClassroomsSL(),
                Days = ConstantStrings.getSchoolDaysSL(),
                Hours = ConstantStrings.getSchoolHoursSL(),
                Teachers = ConstantStrings.getTeachersSL(),
                ClassroomId = subject.ClassroomId,
                ClasssId = subject.ClasssId,
                Day = subject.Day,
                Hour = subject.Hour,
                Id = subject.Id,
                TeacherId = subject.TeacherId,
                Name = subject.Name
            };

            return View(subjectVm);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SubjectCreateViewModel subjectVm)
        {
            if (ModelState.IsValid)
            {
                var subject = new Subject
                {
                    ClassroomId = subjectVm.ClassroomId,
                    ClasssId = subjectVm.ClasssId,
                    Day = subjectVm.Day,
                    Hour = subjectVm.Hour,
                    Id = subjectVm.Id,
                    Name = subjectVm.Name,
                    TeacherId = subjectVm.TeacherId
                };

                subjectRepo.Update(subject);
                subjectRepo.Save();
                
                return RedirectToAction("Index");
            }

            return View(subjectVm);
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
    }
}
