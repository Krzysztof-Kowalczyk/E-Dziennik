using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.Models
{
    public class Classs
    {
        public int Id { get; set; }

        public string Name { get; set;}

        public virtual List<Student> Students { get; set; }
    }
}