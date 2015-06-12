using System.Linq;
using Models.Models;

namespace Models.Interfaces
{
    public interface IMarkRepository : IRepository<Mark>
    {
        IQueryable<Mark> FindByStudentIdAndSubjectId(string studentId, int subjectId);
        IQueryable<Mark> FindByStudentId(string studentId);
    }
}
