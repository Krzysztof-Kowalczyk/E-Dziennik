using Repositories.Repositories;
using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using edziennik.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace edziennik.Validators
{
    public class OnlyMarkTeacherOrAdmin : AuthorizeAttribute
    {
        readonly TeacherRepository _teacherRepo = new TeacherRepository();
        readonly MarkRepository _markRepo = new MarkRepository();
        protected ApplicationDbContext ApplicationDbContext { get; set; }
        protected UserManager<ApplicationUser> UserManager { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authorized = base.AuthorizeCore(httpContext);
            if (!authorized)
            {
                return false;
            }

            ApplicationDbContext = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>
                                         (new UserStore<ApplicationUser>(ApplicationDbContext));

            var rd = httpContext.Request.RequestContext.RouteData;
            var id = Convert.ToInt32(rd.Values["id"]);
            var userId = httpContext.User.Identity.GetUserId();

            var user = UserManager.FindById(userId);

            if (UserManager.IsInRole(user.Id, "Admins"))
            {
                return true;
            }

            var mark = _markRepo.FindById(id);

            return mark != null && mark.TeacherId == user.Id;
        }
    }

}