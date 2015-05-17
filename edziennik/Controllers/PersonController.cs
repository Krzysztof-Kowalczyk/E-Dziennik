using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using edziennik.Models;
using edziennik.Resources;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using Models.Models;

namespace edziennik.Controllers
{
    public abstract class PersonController : Controller
    {
        protected ApplicationDbContext ApplicationDbContext { get; set; }
        protected UserManager<ApplicationUser> UserManager { get; set; }

        protected PersonController()
        {
            var provider = new DpapiDataProtectionProvider("Sample");
            ApplicationDbContext = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>
                (new UserStore<ApplicationUser>(ApplicationDbContext))
            {
                UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(
                    provider.Create("EmailConfirmation"))
            };
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
                AvatarUrl = ConstantStrings.DefaultUserAvatar,
                CreateDate = DateTime.Now
            };
            user.LastPasswordChange = user.CreateDate;

            var result = UserManager.Create(user, password);
            if (result.Succeeded)
            {
                UserManager.AddToRole(user.Id, role);
                var code = UserManager.GenerateEmailConfirmationToken(user.Id);
                ApplicationDbContext.Create().SaveChanges();

                var callbackUrl = Url.Action(

                    "ConfirmEmails",

                    "Account",

                    new { userId = user.Id, code = code },

                    protocol: Request.Url.Scheme);

                 SendEmail(

                 user.Email,

                 "Rejestracja konta",

                 "Potwierdź utworzenie konta klikając na podany link: " +
                 "<a href=\"" + callbackUrl + "\">Potwierdź</a>");

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
        protected async Task UpdateUser(ApplicationUser user, Person person)
        {
            var password = person.Surname.Substring(0, 3) +
                                        person.Pesel.Substring(7, 4);

            await UserManager.RemovePasswordAsync(person.Id);
            await UserManager.AddPasswordAsync(person.Id, password);
            user.UserName = person.Pesel;
            user.LastPasswordChange = DateTime.Now;
            await UserManager.UpdateAsync(user);
            ApplicationDbContext.Create().SaveChanges();
        }

        [NonAction]
        private  void SendEmail(string destination, string subject, string body)
        {
            const string credentialUserName = "jedznaplus@gmail.com";
            const string sentFrom = "jedznaplus@gmail.com";
            const string pwd = "jedznaplus123";
            var credentials = new NetworkCredential(credentialUserName, pwd);

            // Configure the client:
            var client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true,
                Credentials = credentials
            };

            // Create the message:
            var mail = new MailMessage(sentFrom, destination)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

           client.Send(mail);
        }


    }
}