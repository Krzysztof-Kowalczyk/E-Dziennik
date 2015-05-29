using Models.Models;
using System.Collections.Generic;
using System.Linq;

namespace Models.Interfaces
{
    public interface IMarkRepository : IRepository<Mark>
    {
        IQueryable<Mark> FindByStudentIdAndSubjectId(string studentId, int subjectId);
    }
}
