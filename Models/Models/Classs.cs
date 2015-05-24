using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Classs
    {
        public int Id { get; set; }

        [Display(Name = "Nazwa")]
        [Required]
        public string Name { get; set;}

        [Display(Name = "Id wychowawcy")]
        public string TeacherId { get; set; }

        [Display(Name = "Lista uczniów")]
        public virtual List<Student> Students { get; set; }
    }
}