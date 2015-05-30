using System;
using System.Linq;

namespace Models.Interfaces
{
    public interface IPersonRepository<T> : IDisposable
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetPage(int? page, int? pageSize); 
        T FindById(string id);
        IQueryable<T> FindBySurname(string surname);
        void Insert(T item);
        void Update(T item);
        void Delete(string id);
        void Save();

    }
}
