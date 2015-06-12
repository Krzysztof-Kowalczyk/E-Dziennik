using System;
using System.Linq;
using Models.Interfaces;
using Models.Models;

namespace Repositories.Repositories
{
    public class ClasssRepository : IClasssRepository 
    {
        readonly EDziennikContext _db = new EDziennikContext();
        public IQueryable<Classs> GetAll()
        {
            return _db.Classes.AsNoTracking();
        }

        public IQueryable<Classs> GetPage(int? page = 1, int? pageSize = 10)
        {
            var items = _db.Classes.OrderByDescending(o => o.Id).Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

            return items;
        } 

        public Classs FindById(int id)
        {
            return _db.Classes.SingleOrDefault(a => a.Id == id);
        }

        public void Insert(Classs item)
        {
            _db.Classes.Add(item);
        }

        public void Update(Classs item)
        {
            var classs = _db.Classes.Single(a => a.Id == item.Id);
            classs.Id = item.Id;
            classs.Name = item.Name;
            classs.Students = item.Students;
        }

        public void Delete(int id)
        {
            var classs=_db.Classes.Single(a => a.Id == id);
            _db.Classes.Remove(classs);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
       
        public Classs FindByStudentId(string studentId)
        {
            var student = _db.Students.Single(a => a.Id == studentId);
           
            return _db.Classes.Single(a => a.Id == student.ClasssId);
        }

        public IQueryable<Classs> FindByTeacherId(string teacherId)
        {
            var classes = _db.Classes.Where(a => a.TeacherId == teacherId);

            return classes;
        }

        public  Classs FindByMarkId(int markId)
        {
            var mark = _db.Marks.Single(a => a.Id == markId);

            var student = _db.Students.Single(a=>a.Id == mark.StudentId);

            return _db.Classes.Single(a=>a.Id == student.ClasssId);

        }

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}