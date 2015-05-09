using System.Collections.Generic;
using System.Linq;
using Models.Interfaces;
using Models.Models;

namespace Repositories.Repositories
{
    public class ClasssRepository : IClasssRepository
    {
        EDziennikContext db = new EDziennikContext();
        public List<Classs> GetAll()
        {
            return db.Classes.ToList();
        }

        public Classs FindById(int id)
        {
            return db.Classes.Single(a => a.Id == id);
        }

        public void Insert(Classs item)
        {
            db.Classes.Add(item);
        }

        public void Update(Classs item)
        {
            var classs = db.Classes.Single(a => a.Id == item.Id);
            classs.Id = item.Id;
            classs.Name = item.Name;
            classs.Students = item.Students;
        }

        public void Delete(int id)
        {
            var classs=db.Classes.Single(a => a.Id == id);
            db.Classes.Remove(classs);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}