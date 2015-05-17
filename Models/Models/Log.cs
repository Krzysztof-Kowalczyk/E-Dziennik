using System;
using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Log
    {
        public int Id { get; set; }

        [Display(Name = "Data")]
        public DateTime Date { get; set; }

        [Display(Name = "Akcja")]
        public string Action { get; set; }

        [Display(Name = "Kto")]
        public string Who { get; set; }

        [Display(Name = "Co")]
        public string What { get; set; }

        [Display(Name = "Komu")]
        public string WhatId { get; set; }

    }
}