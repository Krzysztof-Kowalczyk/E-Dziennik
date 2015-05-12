using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Validators;

namespace Models.Models
{
    public class Person
    {
        [Unique]
        public string Id { get; set; }
        [Display (Name= "Imię")]
        public string FirstName { get; set; }
        [Display(Name = "Drugie imię")]
        public string SecondName { get; set; }
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }
       
        [Display(Name = "Pesel")]
        [Pesel]
        public string Pesel {get; set;}

        [NotMapped]
        public string FullName
        {
            get { return FirstName + " " + SecondName + " " + Surname; }
        }

    }
}