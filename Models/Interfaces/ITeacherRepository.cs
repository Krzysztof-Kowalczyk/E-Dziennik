using System.Collections.Generic;
using Models.Models;

namespace Models.Interfaces
{
    public interface ITeacherRepository : IPersonRepository<Teacher>
    {
        List<Classs> GetClasses(string teacherId);
        List<Subject> GetSubjects(string teacherId);
    }
}
