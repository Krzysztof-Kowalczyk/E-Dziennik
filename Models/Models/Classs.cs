using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Classs
    {
        public Classs()
        {
            Students = new List<Student>();
        }
        public int Id { get; set; }

        [Display(Name = "Nazwa")]
        public string Name { get; set;}

        [Display(Name = "Id wychowawcy")]
        public int TeacherId { get; set; }

        [Display(Name = "Lista uczniów")]
        public virtual List<Student> Students { get; set; }
    }
}