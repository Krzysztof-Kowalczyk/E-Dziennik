using System.Collections.Generic;

namespace Models.Interfaces
{
    public interface IRepository<T>
    {
        List<T> GetAll();
        T FindById(int id);
        void Insert(T item);
        void Update(T item);
        void Delete(int id);
        void Save();
    }
}
