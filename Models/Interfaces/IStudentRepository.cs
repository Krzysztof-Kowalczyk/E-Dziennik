﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Models;

namespace Models.Interfaces
{
    public interface IStudentRepository : IPersonRepository<Student>
    {
        List<Mark> GetMarks(string studentId);
    }
}
