using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.Interfaces
{
    public interface IPersonRepository<T> : IDisposable
    {
        List<T> GetAll();
        List<T> GetPage(int? page, int? pageSize); 
        T FindById(string id);
        List<T> FindBySurname(string surname);
        void Insert(T item);
        void Update(T item);
        void Delete(string id);
        void Save();

    }
}
