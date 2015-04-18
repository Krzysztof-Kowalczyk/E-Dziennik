using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.Models
{
    public class Teacher: Person
    {
        public virtual List<Classs> ClassList { get; set; }

        public virtual List<Classs> SubjectList { get; set; }
    }
}