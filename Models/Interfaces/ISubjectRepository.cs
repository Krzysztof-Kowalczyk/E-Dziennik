using System.Linq;
using Models.Models;

namespace Models.Interfaces
{
    public interface ISubjectRepository : IRepository<Subject>
    {
        IQueryable<Subject> FindByClassId(int classId);
        IQueryable<Subject> FindByTeacherId(string teacherId);
        IQueryable<Subject> FindByStudentId(string studentId);
    }
}
