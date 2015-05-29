using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Models.Models;
using System.Linq;

namespace Models.Interfaces
{
    public interface ISubjectRepository : IRepository<Subject>
    {
        IQueryable<Subject> FindByClassId(int classId);
        IQueryable<Subject> FindByTeacherId(string teacherId);
        IQueryable<Subject> FindByStudentId(string studentId);
    }
}
