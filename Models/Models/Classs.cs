﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models.Models
{
    public class Classs
    {
        public int Id { get; set; }

        [Display(Name = "Nazwa")]
        public string Name { get; set;}

        [Display(Name = "Id wychowawcy")]
        public int TutorId { get; set; }

        [Display(Name = "Lista uczniów")]
        public virtual List<Student> Students { get; set; }
    }
}