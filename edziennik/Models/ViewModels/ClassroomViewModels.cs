using System.ComponentModel.DataAnnotations;

namespace edziennik.Models.ViewModels
{
    public class ClassroomDetailsViewModel 
    {
        public int Id { get; set; }

        [Display(Name = "Nazwa")]
        [Required]
        public string Name { get; set; }
       
        [Display(Name="Liczba zajęć w tygodniu")]
        public int SubjectsCount { get; set; }

    }
}