using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models.Models
{
    public class Mark
    {
        public int Id { get; set; }

        [Display(Name = "Wartość")]
        public double Value { get; set; }

        [Display(Name = "Id ucznia")]
        public string StudentId { get; set; }

        [Display(Name = "Id przedmiotu")]
        public int SubjectId { get; set; }

        [Display(Name = "Id nauczyciela")]
        public string TeacherId { get; set; }
    }
}