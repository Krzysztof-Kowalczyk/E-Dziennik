using System.ComponentModel.DataAnnotations;
using Models.Models;

namespace edziennik.Models.ViewModels
{
    public class SubjectViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Display(Name = "Nauczyciel")]
        public string Teacher { get; set; }

        [Display(Name = "Klasa")]
        public string Classs { get; set; }

        [Display(Name = "Sala")]
        public string Classroom { get; set; }

        [Display(Name = "Dzień zajęć")]
        public SchoolDay Day { get; set; }

        [Display(Name = "Godzina zajęć")]
        public int Hour { get; set; }
    }

    public class TeacherSubjectViewModel : SubjectViewModel
    {
        public string TeacherId { get; set; }
    }

    public class StudentSubjectViewModel : SubjectViewModel
    {
        public string StudentId { get; set; }

        public string Student { get; set; }
    }

    public class ClassSubjectViewModel : SubjectViewModel
    {
        public int ClassId { get; set; }
    }
}