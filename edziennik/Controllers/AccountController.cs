using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using edziennik.Models;
using edziennik.Resources;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace edziennik.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationSignInManager _signInManager;
        private readonly IAuthenticationManager _authenticationManager;

        public AccountController(ApplicationUserManager userManager,
                                 ApplicationSignInManager signInManager, IAuthenticationManager authenticationManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationManager = authenticationManager;
        }

        ////////////////////

        public ActionResult DisplayPhoto()
        {
            ViewBag.FilePath = ConstantStrings.DefaultUserAvatar;
            return PartialView("_DisplayPhoto");
        }

        [Authorize(Roles = "Admins, Editors")]
        public ActionResult ShowUsers(int? page, string sortOrder)
        {
            // var users = SortItems(sortOrder);
            var usersVm = _userManager.Users.ToList().Select(u => new UserListItemViewModel
            {
                Id = u.Id,
                Email = u.Email,
                UserName = u.UserName,
                EmailConfirmed = u.EmailConfirmed,
                Role = ApplicationDbContext.Create().Roles.ToList().Single(a => a.Id == u.Roles.ElementAt(0).RoleId).Name
            }).ToList();

            return View(usersVm);
        }

        [Authorize(Roles = "Admins")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admins")]
        public async Task<ActionResult> Create(UserCreateViewModel ruser, string role)
        {
            if (ModelState.IsValid)
            {
                var hasher = new PasswordHasher();
                const string password = "Editor123#";
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

                var result = _userManager.Create(user, password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user.Id, "Editors");

                    return RedirectToAction("ShowUsers");
                }
                AddErrors(result);
            }
            return View();
        }

        [Authorize(Roles = "Admins,Editors")]
        public ActionResult Details(string id)
        {
            var user = _userManager.FindById(id);
            var vm = new UserDetailsViewModel
            {
                Id = user.Id,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                UserName = user.UserName,
                UserRoles = _userManager.GetRoles(user.Id).ToArray(),
                AvatarUrl = user.AvatarUrl,
                Roles = new List<SelectListItem>
                {
                    new SelectListItem{Value = "Admins", Text="Admins"},
                    new SelectListItem{Value = "Editors", Text="Editors"}
                } 
            };

            return View(vm);
        }

        [NonAction]
        public void DeleteAvatar(string relativePath)
        {
            if (relativePath != ConstantStrings.DefaultUserAvatar)
            {
                var path = Server.MapPath(relativePath);
                System.IO.File.Delete(path);
            }
        }

        public ActionResult DeleteImage(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var toDelete = _userManager.FindById(id);

            if (toDelete != null)
            {
                DeleteAvatar(toDelete.AvatarUrl);
                toDelete.AvatarUrl = ConstantStrings.DefaultUserAvatar;
                _userManager.Update(toDelete);
            }

            return RedirectToAction("Edit", toDelete);

        }

        [Authorize(Roles = "Admins")]
        public ActionResult Edit(string id)
        {
            var user = _userManager.FindById(id);

            var vm = new UserEditViewModel
            {
                Id = user.Id,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                UserName = user.UserName,
                UserRoles = _userManager.GetRoles(user.Id).ToArray(),
                AvatarUrl = user.AvatarUrl,
                Roles = new List<SelectListItem>
                {
                    new SelectListItem{Value = "Admins", Text="Admins"},
                    new SelectListItem{Value = "Editors", Text="Editors"}
                }
            };

            return View(vm);
        }

        [Authorize(Roles = "Admins")]
        [HttpPost]
        public ActionResult Edit(UserEditViewModel user, HttpPostedFileBase file)
        {
            if (ModelState.IsValid && user.UserRoles != null)
            {
                var dbPost = _userManager.FindById(user.Id);
                if (dbPost == null)
                {
                    return HttpNotFound();
                }

                dbPost.UserName = user.UserName;
                dbPost.Email = user.Email;
                dbPost.EmailConfirmed = user.EmailConfirmed;
                _userManager.RemoveFromRoles(user.Id, _userManager.GetRoles(user.Id).ToArray());
                _userManager.AddToRoles(user.Id, user.UserRoles);

                if (file != null && file.ContentLength > 0 && file.ContentLength < 3000000)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var uniqueFileName = Guid.NewGuid() + fileName;
                    var absolutePath = Path.Combine(Server.MapPath(ConstantStrings.UserAvatarsPath), uniqueFileName);
                    var relativePath = ConstantStrings.UserAvatarsPath + uniqueFileName;
                    file.SaveAs(absolutePath);
                    dbPost.AvatarUrl = relativePath;
                }
                _userManager.Update(dbPost);

                return RedirectToAction("Details", dbPost);
            }
            user.UserRoles = _userManager.GetRoles(user.Id).ToArray();

            return View(user);
        }

        [Authorize(Roles = "Admins")]
        public async Task<ActionResult> Delete(string id)
        {
            if (await _userManager.IsInRoleAsync(id, "Admins"))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = await _userManager.FindByIdAsync(id);

            var vm = new UserDetailsViewModel
            {
                Id = user.Id,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                UserName = user.UserName,
                UserRoles = _userManager.GetRoles(user.Id).ToArray(),
                Roles = ApplicationDbContext.Create().Roles.Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name
                }).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admins")]
        [ActionName("Delete")]
        public async Task<ActionResult> DeletePost(string id)
        {
            if (await _userManager.IsInRoleAsync(id, "Admins"))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);

            return RedirectToAction("ShowUsers");
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true          
            var user = await _userManager.FindByNameAsync(model.Login);
            var check = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!user.EmailConfirmed)
            {
                ViewBag.UserId = user.Id;
                return View("EmailNotConfirmed");
            }

            if (check && user.UserName != "Admin" && user.LastPasswordChange == user.CreateDate)
            {
                return RedirectToAction("ChangePassword");
            }

            var result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, model.RememberMe, shouldLockout: true);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Wprowadzono błędną Nazwę użytkownika lub hasło");
                    return View(model);
            }
        }

        [AllowAnonymous]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(FirstChangePasswordViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(vm.Login);
                var check = await _userManager.CheckPasswordAsync(user, vm.OldPassword);
                if (check)
                {
                    var result = await _userManager.ChangePasswordAsync(user.Id, vm.OldPassword, vm.NewPassword);
                    if (result.Succeeded)
                    {
                        user.LastPasswordChange = DateTime.Now;
                        _userManager.Update(user);
                        await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                        return RedirectToAction("Index", "Home");
                    }

                    AddErrors(result);
                }
                else
                    ModelState.AddModelError(String.Empty, "Podano złe dane");
            }

            return View(vm);
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await _signInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(await _signInManager.GetVerifiedUserIdAsync());
            if (user != null)
            {
                var code = await _userManager.GenerateTwoFactorTokenAsync(user.Id, provider);
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await _signInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Błędny kod uwierzytelnijący");
                    return View(model);
            }
        }

        [NonAction]
        private async Task SendEmailToken(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user.Id);

            var callbackUrl = Url.Action(

                "ConfirmEmail",

                "Account",

                new { userId = user.Id, code = code },

                protocol: Request.Url.Scheme);

            ServicePointManager.ServerCertificateValidationCallback =
                (s, certificate, chain, sslPolicyErrors) => true;

            await _userManager.SendEmailAsync(

                user.Id,

                "Rejestracja konta",

                "Twoje hasło to trzy pierwsze litery nazwiska (pierwsza litera duża) + 4 ostatnie cyfry numer pesel + #." +
                " Przykładowo hasło dla uzytkownika Jan Kowlaski numer pesel:12345678910, byłoby nastepujące: Kow8910# ." +
                " Potwierdź swoją rejestracje klikając na podany link: " +
                "<a href=\"" + callbackUrl + "\">Potwierdź</a>");
        }

        [AllowAnonymous]
        public async Task<ActionResult> SendEmailConfirmationToken(string userId)
        {
            await SendEmailToken(userId);

            return View();
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var password = model.Surname.Substring(0, 3) + model.Login.Substring(6, 4);
                var user = new ApplicationUser { UserName = model.Login, Email = model.Email, AvatarUrl = ConstantStrings.DefaultUserAvatar };
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user.Id, "Users");

                    await SendEmailToken(user.Id);
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                // var user = await userManager.FindByNameAsync(model.);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    //return View("ForgotPasswordConfirmation");
                    return View("ForgotPasswordFail");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await _userManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await _userManager.SendEmailAsync(user.Id, "Resetowanie hasła", "Aby zresetować hasło kliknij na podany link:  <a href=\"" + callbackUrl + "\">Reset</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            var result = await _userManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        public string AvatarUrl(string id)
        {
            var user = _userManager.FindById(id);
            return user != null ? user.AvatarUrl : ConstantStrings.DefaultUserAvatar;
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await _signInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await _signInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await _authenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await _signInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _authenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            _authenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        public bool IsUniquePesel(string pesel)
        {
            var user = _userManager.FindByName(pesel);

            return user == null;
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}