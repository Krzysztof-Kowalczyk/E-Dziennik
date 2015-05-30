using System.ComponentModel.DataAnnotations;

namespace edziennik.Models.ViewModels
{
    public class TeacherDetailsViewModel : PersonViewModel
    {
        [Display(Name = "Email potwierdzony")]
        public bool EmailConfirmed { get; set; }

        [Display(Name = "Zdjęcie")]
        public string AvatarUrl { get; set; }
    }

    public class TeacherEditViewModel : RegisterViewModel
    {
        [Display(Name = "Zdjęcie")]
        public string AvatarUrl { get; set; }
        public string Id { get; set; }
    }


    public class TeacherRegisterViewModel : RegisterViewModel
    {

    }
}