using System.Collections.Generic;
using Models.Models;

namespace Models.Interfaces
{
    public interface IStudentRepository : IPersonRepository<Student>
    {
        List<Mark> GetMarks(string studentId);
        Student FindByMarkId(int markId);
       
    }
}
