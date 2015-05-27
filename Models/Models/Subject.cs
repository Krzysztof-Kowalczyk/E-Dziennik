using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Subject
    {
        public int Id { get; set; }

        [Display(Name = "Nazwa")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Id nauczyciela")]
        public string TeacherId { get; set; }

        [Display(Name = "Id klasy")]
        public int ClasssId { get; set; }

        [Display(Name = "Id sali")]
        public int ClassroomId { get; set; }

        [Display(Name = "Dzień zajęć")]
        [Required]
        public SchoolDay Day { get; set; }

        [Display(Name = "Godzina zajęć")]
        [Required]
        public int Hour { get; set; }
    }
}