using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Student : Person
    {
        [Display(Name = "Id klasy")]
        public int ClasssId { get; set; }

        [Display(Name = "Numer telefonu komórkowego rodzica")]
        [RegularExpression(@"\d{9}", ErrorMessage = "Niepoprawny numer !")]
        public string CellPhoneNumber { get; set; }

        [Display(Name = "Lista ocen")]
        public virtual List<Mark> Marks { get; set; }
    }
}