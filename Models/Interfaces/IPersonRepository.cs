using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Interfaces
{
    public interface IPersonRepository<T>
    {
        List<T> GetAll();
        T FindById(string id);
        void Insert(T item);
        void Update(T item);
        void Delete(string id);
        void Save();
    }
}
