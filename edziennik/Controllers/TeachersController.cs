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
    public class TeachersController : Controller
    {
        private readonly TeacherRepository repo;
        protected ApplicationDbContext ApplicationDbContext { get; set; }
        protected UserManager<ApplicationUser> UserManager { get; set; }

        public TeachersController(TeacherRepository _repo)
        {
            repo = _repo;
            ApplicationDbContext = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>
                (new UserStore<ApplicationUser>(ApplicationDbContext));
        }

        // GET: Teachers
        public ActionResult Index()
        {
            return View(repo.GetAll());
        }

        // GET: Teachers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = repo.FindById(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        public string CreateUser(RegisterViewModel ruser)
        {
            var hasher = new PasswordHasher();
            var password = ruser.Surname.Substring(0, 3) + ruser.Login.Substring(6, 4);
            var user = new ApplicationUser
            {
                UserName = ruser.Login,
                PasswordHash = hasher.HashPassword(password),
                Email = ruser.Email,
                EmailConfirmed = true,
                AvatarUrl = ConstantStrings.DefaultUserAvatar
            };

            var result = UserManager.Create(user, password);
            if (result.Succeeded)
            {
                UserManager.AddToRole(user.Id, "Teachers");
                ApplicationDbContext.Create().SaveChanges();

                return user.Id;
            }
            AddErrors(result);
            return "Error";
        }

        // GET: Teachers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TeacherRegisterViewModel teacherVM)
        {
            if (ModelState.IsValid)
            {
                var userid = CreateUser(teacherVM);
                if (userid != "Error")
                {
                    var teacher = new Teacher()
                    {
                        Id = userid,
                        FirstName = teacherVM.FirstName,
                        SecondName = teacherVM.SecondName,
                        Surname = teacherVM.Surname,
                        Pesel = teacherVM.Login
                    };
                    repo.Insert(teacher);
                    repo.Save();
                    return RedirectToAction("Index");
                }
            }

            return View(teacherVM);
        }

        // GET: Teachers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = repo.FindById(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,SecondName,Surname,Pesel")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                repo.Update(teacher);
                repo.Save();
                return RedirectToAction("Index");
            }
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = repo.FindById(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            repo.Delete(id);
            repo.Save();
            return RedirectToAction("Index");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

       /* protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repo.Dispose();
            }
            base.Dispose(disposing);
        }*/
    }
}
