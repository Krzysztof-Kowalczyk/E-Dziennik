using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using edziennik.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace edziennik.Validators
{
    public class UniquePesel : ValidationAttribute, IClientValidatable
    {   
        protected ApplicationDbContext ApplicationDbContext { get; set; }
        protected UserManager<ApplicationUser> UserManager { get; set; }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new[] { new ModelClientValidationRule()
            {
                ValidationType = "uniquepesel",
                ErrorMessage = ErrorMessage
            }
            };
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            ApplicationDbContext = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>
                                         (new UserStore<ApplicationUser>(ApplicationDbContext));

            var pesel = (string) value;
            var user = UserManager.FindByName(pesel);

            return user == null;
        }
    }
}