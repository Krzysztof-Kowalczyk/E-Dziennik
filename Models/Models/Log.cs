using System;
using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Log
    {
        public int Id { get; set; }

        [Display(Name = "Data")]
        [Required]
        public DateTime Date { get; set; }

        [Display(Name = "Akcja")]
        [Required]
        public string Action { get; set; }

        [Display(Name = "Kto")]
        [Required]
        public string Who { get; set; }

        [Display(Name = "Co")]
        [Required]
        public string What { get; set; }

        [Display(Name = "Komu")]
        [Required]
        public string WhatId { get; set; }

        [Display(Name = "Adres Ip")]
        [Required]
        public string Ip { get; set; }

    }
}