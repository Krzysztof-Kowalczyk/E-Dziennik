using System.Collections.Generic;
using System.Linq;
using Models.Interfaces;
using Models.Models;
using System;

namespace Repositories.Repositories
{
    public class ClasssRepository : IClasssRepository, IDisposable 
    {
        EDziennikContext db = new EDziennikContext();
        public List<Classs> GetAll()
        {
            return db.Classes.ToList();
        }

        public List<Classs> GetPage(int? page = 1, int? pageSize = 10)
        {
            var items = db.Classes.OrderByDescending(o => o.Id).Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();

            return items;
        } 

        public Classs FindById(int id)
        {
            return db.Classes.SingleOrDefault(a => a.Id == id);
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
       
        public Classs FindByStudentId(string studentId)
        {
            var student = db.Students.Single(a => a.Id == studentId);
           
            return db.Classes.Single(a => a.Id == student.ClasssId);
        }

       public  Classs FindByMarkId(int markId)
        {
            var mark = db.Marks.Single(a => a.Id == markId);

            var student = db.Students.Single(a=>a.Id == mark.StudentId);

            return db.Classes.Single(a=>a.Id == student.ClasssId);

        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}