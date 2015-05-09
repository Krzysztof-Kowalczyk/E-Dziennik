using System.Collections.Generic;

namespace Models.Interfaces
{
    public interface IPersonRepository<T>
    {
        List<T> GetAll();
        T FindById(string id);
        List<T> FindBySurname(string surname);
        void Insert(T item);
        void Update(T item);
        void Delete(string id);
        void Save();
    }
}
