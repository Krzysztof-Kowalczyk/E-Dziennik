using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.Models
{
    public class Student :Person
    {
        public int ClassId { get; set; }

        public int Number { get; set; }

        public virtual List<Mark> Marks { get; set; }
    }
}