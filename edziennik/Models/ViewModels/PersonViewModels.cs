using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace edziennik.Models.ViewModels
{
    public class PersonViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Imię")]
        public string FirstName { get; set; }
        [Display(Name = "Drugie imię")]
        public string SecondName { get; set; }
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }
        [Display(Name = "Pesel")]
        public string Pesel { get; set; }
    }
}