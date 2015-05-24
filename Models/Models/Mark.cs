using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Mark
    {
        public int Id { get; set; }

        [Display(Name = "Wartość")]
        [Required]
        public double Value { get; set; }

        [Display(Name = "Id ucznia")]
        [Required]
        public string StudentId { get; set; }

        [Display(Name = "Id przedmiotu")]
        [Required]
        public int SubjectId { get; set; }

        [Display(Name = "Id nauczyciela")]
        [Required]
        public string TeacherId { get; set; }

        [Display(Name = "Opis")]
        [Required]
        public string Description { get; set; }

    }
}