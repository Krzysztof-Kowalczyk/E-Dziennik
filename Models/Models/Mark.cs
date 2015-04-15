using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.Models
{
    public class Mark
    {
        public int Id { get; set; }

        public double Value { get; set; }

        public string StudentId { get; set; }

        public int SubjectId { get; set; }
    }
}