using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.Models
{
    public class LessonDay
    {
        public SchoolDay SchoolDay { get; set; }
        public int[] Lessons { get; set; }

        public LessonDay(SchoolDay sd)
        {
            SchoolDay = sd;
            Lessons = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12};
        }

    }
}