using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using edziennik.Models;
using edziennik.Resources;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

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

       protected void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

       protected string CreateUser(RegisterViewModel ruser, string role)
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
               UserManager.AddToRole(user.Id, role);
               ApplicationDbContext.Create().SaveChanges();

               return user.Id;
           }
           AddErrors(result);
           return "Error";
       }

    }
}