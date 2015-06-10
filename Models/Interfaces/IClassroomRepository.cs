using System.Linq;
using Models.Models;

namespace Models.Interfaces
{
    public interface IClassroomRepository : IRepository<Classroom>
    {
        IQueryable<Subject> GetSubjects(int classroomId);
    }
}
