using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
