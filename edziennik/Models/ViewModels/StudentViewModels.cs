using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Models.Models;

namespace edziennik.Models.ViewModels
{
    public class SubjectCreateViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nazwa")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Id nauczyciela")]
        [Required]
        public string TeacherId { get; set; }

        [Display(Name = "Id klasy")]
        [Required]
        public int ClasssId { get; set; }

        [Display(Name = "Id sali")]
        [Required]
        public int ClassroomId { get; set; }

        [Display(Name = "Dzień zajęć")]
        [Required]
        public SchoolDay Day { get; set; }

        [Display(Name = "Godzina zajęć")]
        [Required]
        public int Hour { get; set; }

        public List<SelectListItem> Teachers { get; set; }

        public List<SelectListItem> Classrooms { get; set; }

        public List<SelectListItem> Classes { get; set; }

        public List<SelectListItem> Days { get; set; }

        public List<SelectListItem> Hours { get; set; }

    }
    public class StudentDetailsViewModel : PersonViewModel
    {
        [Display(Name = "Klasa")]
        public string ClassName { get; set; }

        [Display(Name = "Numer telefonu komórkowego rodzica")]
        public string CellPhoneNumber { get; set; }

        [Display(Name = "Email potwierdzony")]
        public bool EmailConfirmed { get; set; }

        [Display(Name = "Oceny")]
        public List<MarkViewModel> Marks { get; set; }

        [Display(Name = "Zdjęcie")]
        public string AvatarUrl { get; set; }

    }

    public class StudentListItemViewModel : PersonViewModel
    {
        [Display(Name = "Klasa")]
        public string ClassName { get; set; }
    }

    public class StudentRegisterViewModel : RegisterViewModel
    {
        [Display(Name = "Klasa")]
        public int ClassId { get; set; }

        [Display(Name = "Numer telefonu komórkowego rodzica")]
        [RegularExpression(@"\d{9}", ErrorMessage = "Niepoprawny numer !")]
        public string CellPhoneNumber { get; set; }

        public List<SelectListItem> Classes { get; set; }
    }

    public class StudentEditViewModel : StudentRegisterViewModel
    {
        [Display(Name = "Zdjęcie")]
        public string AvatarUrl { get; set; }
        public string Id { get; set; }
    }

    public class StudentSubjectMarks
    {
        public string Id { get; set; }

        [Display(Name = "Imię")]
        public string FirstName { get; set; }
        [Display(Name = "Drugie imię")]
        public string SecondName { get; set; }
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }
        public List<Mark> Marks { get; set; }
    }
}