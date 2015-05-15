using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace edziennik.Validators
{
    public class PeselAttribute : ValidationAttribute, IClientValidatable
    {
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new[] { new ModelClientValidationRule
            {
                ValidationType = "pesel",
                ErrorMessage = ErrorMessage
            }
            };
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;
            var weights = new[] { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
            var str = value as string;
            int ctr = (10 - str.Take(10).Select((ch, i) => int.Parse(ch.ToString()) * weights[i]).Sum() % 10) % 10;
            return ctr == int.Parse(str[10].ToString());
        }
    }
}