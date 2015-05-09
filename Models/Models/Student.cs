using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Student : Person
    {
        [Display(Name = "Id klasy")]
        public int ClasssId { get; set; }

        [Display(Name = "Lista ocen")]
        public virtual List<Mark> Marks { get; set; }
    }
}