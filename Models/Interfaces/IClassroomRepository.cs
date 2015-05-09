using System.Collections.Generic;
using Models.Models;

namespace Models.Interfaces
{
    public interface IClassroomRepository : IRepository<Classroom>
    {
        List<Subject> GetSubjects(int classroomId);
    }
}
