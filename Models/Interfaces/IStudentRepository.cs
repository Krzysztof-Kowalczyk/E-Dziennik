using Models.Models;

namespace Models.Interfaces
{
    public interface IStudentRepository : IPersonRepository<Student>
    {
        Student FindByMarkId(int markId);      
    }
}
