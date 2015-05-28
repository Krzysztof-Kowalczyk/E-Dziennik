using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace edziennik.Models.ViewModels
{
    public class LogListItemViewModel
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

    }

    public class LogDetailsViewModel : LogListItemViewModel
    {
        [Display(Name = "Adres Ip")]
        public string Ip { get; set; }

        [Display(Name = "Komu")]
        public string WhatId { get; set; }
    }
}