using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace edziennik.Models.ViewModels
{
    public class MarkCreateViewModel
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

        public List<SelectListItem> Subjects { get; set; }

        public List<SelectListItem> Values { get; set; }
    }

    public class MarkViewModel
    {
        [Display(Name = "Nauczyciel")]
        public string Teacher { get; set; }

        [Display(Name = "Przedmiot")]
        public string Subject { get; set; }

        [Display(Name = "Ocena")]
        public double Value { get; set; }
    }

    public class MarkListItemViewModel : MarkViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nauczyciel")]
        public string TeacherId { get; set; }

        [Display(Name = "Klasa")]
        public string Classs { get; set; }

        [Display(Name = "Uczeń")]
        public string Student { get; set; }

    }

    public class MarkDetailsViewModel : MarkListItemViewModel
    {
        [Display(Name = "Opis")]
        public string Description { get; set; }
    }
}