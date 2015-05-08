using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models.Models;

namespace Models.Models
{
    public enum SchoolDay
    {
        Monday,
        Thuesday,
        Wednesday,
        Thursday,
        Friday
    }

    public class LessonHour
    {
        public SchoolDay SchoolDay {get; set;}
        public int Lesson { get; set; }

    }
}