using System.Web.Mvc;
using edziennik.Models;
using edziennik.Resources;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Models.Models;

namespace edziennik.Controllers
{
    public abstract class PersonController : Controller
    {
        protected ApplicationDbContext ApplicationDbContext { get; set; }
        protected UserManager<ApplicationUser> UserManager { get; set; }

        protected PersonController()
        {
            ApplicationDbContext = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>
            (new UserStore<ApplicationUser>(ApplicationDbContext));
        }
       
       [NonAction]
       protected void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

       [NonAction]
       protected string CreateUser(RegisterViewModel ruser, string role)
       {
           var hasher = new PasswordHasher();
           var password = ruser.Surname.Substring(0, 3) + ruser.Login.Substring(7, 4);
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
               UserManager.AddToRole(user.Id, role);
               ApplicationDbContext.Create().SaveChanges();

               return user.Id;
           }
           AddErrors(result);
           return "Error";
       }

       [NonAction]
       protected void DeleteUser(string id)
       {
           var user = UserManager.FindById(id);
           UserManager.Delete(user);
           ApplicationDbContext.Create().SaveChanges();
       }

        [NonAction]
        protected void UpdateUser(ApplicationUser user, Person person)
        {
            var password = person.Surname.Substring(0, 3) +
                                        person.Pesel.Substring(7, 4);

            UserManager.RemovePassword(person.Id);
            UserManager.AddPassword(person.Id, password);
            user.UserName = person.Pesel;
            UserManager.Update(user);
            ApplicationDbContext.Create().SaveChanges();
        }


    }
}