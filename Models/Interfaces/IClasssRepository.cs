using System.Linq;
using Models.Models;

namespace Models.Interfaces
{
    public interface IClasssRepository : IRepository<Classs>
    {
        Classs FindByStudentId(string studentId);
        IQueryable<Classs> FindByTeacherId(string teacherId);
        Classs FindByMarkId(int markId);
    }
}
