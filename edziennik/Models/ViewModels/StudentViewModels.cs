using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace edziennik.Models.ViewModels
{

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

    public class ClassStudentViewModel : StudentListItemViewModel
    {
        public int ClassId { get; set; }
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

}