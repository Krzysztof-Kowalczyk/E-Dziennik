using System.Linq;
using System.Net;
using System.Web.Mvc;
using edziennik.Models;
using edziennik.Resources;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Models.Models;
using Repositories.Repositories;

namespace edziennik.Controllers
{
    public class StudentsController : Controller
    {
        private readonly StudentRepository repo;
        
        private ClasssRepository crepo = new ClasssRepository();
        protected ApplicationDbContext ApplicationDbContext { get; set; }
        protected UserManager<ApplicationUser> UserManager { get; set; }

        public StudentsController(StudentRepository _repo)
        {
            repo = _repo;
            ApplicationDbContext = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>
                (new UserStore<ApplicationUser>(ApplicationDbContext));

        }

        // GET: Students
        public ActionResult Index()
        {
            return View(repo.GetAll());
        }

        // GET: Students/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = repo.FindById(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        public string CreateUser(RegisterViewModel ruser)
        {
            var hasher = new PasswordHasher();
            var user = new ApplicationUser
            {
                UserName = ruser.Login,
                PasswordHash = hasher.HashPassword(ruser.Password),
                Email = ruser.Email,
                EmailConfirmed = true,
                AvatarUrl = ConstantStrings.DefaultUserAvatar
            };

            UserManager.Create(user, ruser.Password);
            UserManager.AddToRole(user.Id,"Students");
            ApplicationDbContext.Create().SaveChanges();

            return user.Id;
        }

        // GET: Students/Create
        public ActionResult Create()
        {
             var klasa = crepo.GetAll().Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.Name
            });

            ViewBag.ClassId= new SelectList(crepo.GetAll(), "Id", "Name");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StudentRegisterViewModel studentVM)
        {
            if (ModelState.IsValid)
            {
                var userid = CreateUser(studentVM);
                var student = new Student()
                {
                    Id= userid,
                    ClassId = studentVM.ClassId,
                    FirstName = studentVM.FirstName,
                    SecondName = studentVM.SecondName,
                    Surname = studentVM.Surname,
                    Number = studentVM.Number,
                    Pesel = studentVM.Surname.Substring(1, 3) + studentVM.Login.Substring(6, 4)
                };
                repo.Insert(student);
                repo.Save();
                return RedirectToAction("Index");
            }
           ViewBag.Klasa = crepo.GetAll().Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.Name
            }).ToList();
           ViewBag.ClassId = new SelectList(crepo.GetAll(), "Id", "Name");

           return View(studentVM);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = repo.FindById(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ClassId,Number,FirstName,SecondName,Surname,Pesel")] Student student)
        {
            if (ModelState.IsValid)
            {
                repo.Update(student);
                repo.Save();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = repo.FindById(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            repo.Delete(id);
            repo.Save();
            return RedirectToAction("Index");
        }

       /* protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }*/
    }
}
