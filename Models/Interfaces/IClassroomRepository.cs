using System.Collections.Generic;
using Models.Models;
using System.Linq;

namespace Models.Interfaces
{
    public interface IClassroomRepository : IRepository<Classroom>
    {
        IQueryable<Subject> GetSubjects(int classroomId);
    }
}
