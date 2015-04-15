using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            throw new NotImplementedException();
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