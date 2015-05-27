using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Models.Models;

namespace Models.Interfaces
{
    public interface ISubjectRepository : IRepository<Subject>
    {
        List<Subject> FindByClassId(int classId);
        List<Subject> FindByTeacherId(string teacherId);
        List<Subject> FindByStudentId(string studentId);
    }
}
