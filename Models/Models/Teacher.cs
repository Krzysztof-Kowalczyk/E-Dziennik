using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Models
{
    public class Teacher: Person
    {
        [NotMapped]
        public string FullName 
        {
            get { return FirstName + " " + SecondName + Surname; }
        }

        public virtual List<Classs> ClassList { get; set; }

        public virtual List<Classs> SubjectList { get; set; }
    }
}