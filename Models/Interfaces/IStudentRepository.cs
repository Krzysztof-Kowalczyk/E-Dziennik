using System.Linq;
using Models.Models;

namespace Models.Interfaces
{
    public interface IStudentRepository : IPersonRepository<Student>
    {
        Student FindByMarkId(int markId);
        IQueryable<Student> FindByClassId(int classId);

    }
}
