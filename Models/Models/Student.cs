using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models.Models
{
    public class Student :Person
    {
        [Display(Name = "Id klasy")]
        public int ClassId { get; set; }

        [Display(Name = "Numer w dzienniku")]
        public int Number { get; set; }

        [Display(Name = "Lista ocen")]
        public virtual List<Mark> Marks { get; set; }
    }
}