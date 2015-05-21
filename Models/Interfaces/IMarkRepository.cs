using Models.Models;
using System.Collections.Generic;

namespace Models.Interfaces
{
    public interface IMarkRepository : IRepository<Mark>
    {
        List<Mark> FindByStudentIdAndSubjectId(string studentId, int subjectId);
    }
}
