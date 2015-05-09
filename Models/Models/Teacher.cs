using System.Collections.Generic;

namespace Models.Models
{
    public class Teacher: Person
    {
        public virtual List<Classs> ClassList { get; set; }

        public virtual List<Classs> SubjectList { get; set; }
    }
}