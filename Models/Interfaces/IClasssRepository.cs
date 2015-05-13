using Models.Models;

namespace Models.Interfaces
{
    public interface IClasssRepository : IRepository<Classs>
    {
        Classs FindByStudentId(string studentId);
        Classs FindByMarkId(int markId);
    }
}
