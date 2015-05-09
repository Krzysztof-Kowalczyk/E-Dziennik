using System.Data.Entity;
using edziennik.Models;
using edziennik.Resources;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace edziennik.App_Start
{
    public class UserDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var rs = new RoleStore<IdentityRole>(context);
            var rm = new RoleManager<IdentityRole>(rs);

            rm.Create(new IdentityRole { Name = "Admins" });
            rm.Create(new IdentityRole { Name = "Editors" });
            rm.Create(new IdentityRole { Name = "Teachers" });
            rm.Create(new IdentityRole { Name = "Students" });

            var us = new UserStore<ApplicationUser>(context);
            var um = new UserManager<ApplicationUser>(us);

            var user = new ApplicationUser { UserName = "Admin", Email = "admin@myapp.pl", EmailConfirmed = true, AvatarUrl = ConstantStrings.DefaultUserAvatar };
            um.Create(user, "Admin123#");
            um.AddToRole(user.Id, "Admins");
        }
    }
}