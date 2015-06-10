using System;
using System.Linq;
using Models.Interfaces;
using Models.Models;

namespace Repositories.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        readonly EDziennikContext _db = new EDziennikContext();
        public IQueryable<Teacher> GetAll()
        {
            return _db.Teachers.AsNoTracking();
        }
        public IQueryable<Teacher> GetPage(int? page = 1, int? pageSize = 10)
        {
            var items = _db.Teachers.OrderByDescending(o => o.Id).Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

            return items;
        } 

        public Teacher FindById(string id)
        {
            return _db.Teachers.SingleOrDefault(a => a.Id == id);
        }

        public IQueryable<Teacher> FindBySurname(string surname)
        {
            return _db.Teachers.Where(a => a.Surname == surname.ToLower());
        }

        public void Insert(Teacher item)
        {
            _db.Teachers.Add(item);
        }

        public void Update(Teacher item)
        {
            var teacher= _db.Teachers.Single(a => a.Id == item.Id);
            teacher.FirstName = item.FirstName;
            teacher.SecondName = item.SecondName;
            teacher.Surname = item.Surname;
            teacher.ClassList = item.ClassList;
            teacher.Pesel = item.Pesel;
        }

        public void Delete(string id)
        {
            var teacher = _db.Teachers.Single(a => a.Id == id);
            _db.Teachers.Remove(teacher);
        }

        public void Save()
        {
            _db.SaveChanges();
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