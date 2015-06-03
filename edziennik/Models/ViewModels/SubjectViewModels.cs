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