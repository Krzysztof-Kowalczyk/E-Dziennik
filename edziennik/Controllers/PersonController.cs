using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using edziennik.Models;
using edziennik.Resources;
using Microsoft.AspNet.Identity;
using Models.Models;

namespace edziennik.Controllers
{
    [HandleError]
    public abstract class PersonController : Controller
    {
        protected readonly ApplicationUserManager UserManager;

        protected PersonController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
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
        protected async Task<string> CreateUser(RegisterViewModel ruser, string role)
        {
            var hasher = new PasswordHasher();
            var password = ruser.Surname.Substring(0, 3) + ruser.Login.Substring(7, 4) + "#";
            var user = new ApplicationUser
            {
                UserName = ruser.Login,
                PasswordHash = hasher.HashPassword(password),
                Email = ruser.Email,
                EmailConfirmed = ruser.EmailConfirmed,
                AvatarUrl = ConstantStrings.DefaultUserAvatar,
                CreateDate = DateTime.Now
            };
            user.LastPasswordChange = user.CreateDate;

            var result = UserManager.Create(user, password);
            if (result.Succeeded)
            {
                UserManager.AddToRole(user.Id, role);
                if (!user.EmailConfirmed)
                {
                    await SendEmailActivationToken(user);
                }
                return user.Id;
            }
            AddErrors(result);
            return "Error";
        }

        [NonAction]
        private async Task SendEmailActivationToken(ApplicationUser user)
        {
            var code = UserManager.GenerateEmailConfirmationToken(user.Id);

            var callbackUrl = Url.Action(

                "ConfirmEmail",

                "Account",

                new { userId = user.Id, code = code },

                protocol: Request.Url.Scheme);

            ServicePointManager.ServerCertificateValidationCallback =
                (s, certificate, chain, sslPolicyErrors) => true;

            try
            {
                await UserManager.SendEmailAsync(

                    user.Id,

                    "Rejestracja konta",

                    "Twoje hasło to trzy pierwsze litery nazwiska (pierwsza litera duża) + 4 ostatnie cyfry numer pesel + #." +
                    " Przykładowo hasło dla uzytkownika Jan Kowlaski numer pesel:12345678910, byłoby nastepujące: Kow8910# ." +
                    " Potwierdź swoją rejestracje klikając na podany link: " +
                    "<a href=\"" + callbackUrl + "\">Potwierdź</a>");
            }
            catch (Exception)
            {
                // ignored
            }
        }

        [NonAction]
        protected void DeleteUser(string id)
        {
            var user = UserManager.FindById(id);
            UserManager.Delete(user);
        }

        [NonAction]
        protected async Task UpdateUser(ApplicationUser user, Person person, string email, bool emailConfirmed)
        {
            user.UserName = person.Pesel;
            user.Email = email;
            user.EmailConfirmed = emailConfirmed;
            await UserManager.UpdateAsync(user);
        }

    }
}