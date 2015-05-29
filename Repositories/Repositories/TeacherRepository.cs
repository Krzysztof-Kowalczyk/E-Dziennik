using System.Collections.Generic;
using System.Linq;
using Models.Interfaces;
using Models.Models;
using System;

namespace Repositories.Repositories
{
    public class TeacherRepository : ITeacherRepository, IDisposable
    {
        EDziennikContext db = new EDziennikContext();
        public IQueryable<Teacher> GetAll()
        {
            return db.Teachers.AsNoTracking();
        }
        public IQueryable<Teacher> GetPage(int? page = 1, int? pageSize = 10)
        {
            var items = db.Teachers.OrderByDescending(o => o.Id).Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

            return items;
        } 

        public Teacher FindById(string id)
        {
            return db.Teachers.SingleOrDefault(a => a.Id == id);
        }

        public IQueryable<Teacher> FindBySurname(string surname)
        {
            return db.Teachers.Where(a => a.Surname == surname.ToLower());
        }

        public void Insert(Teacher item)
        {
            db.Teachers.Add(item);
        }

        public void Update(Teacher item)
        {
            var teacher= db.Teachers.Single(a => a.Id == item.Id);
            teacher.FirstName = item.FirstName;
            teacher.SecondName = item.SecondName;
            teacher.Surname = item.Surname;
            teacher.ClassList = item.ClassList;
            teacher.Pesel = item.Pesel;
        }

        public void Delete(string id)
        {
            var teacher = db.Teachers.Single(a => a.Id == id);
            db.Teachers.Remove(teacher);
        }

        public void Save()
        {
            db.SaveChanges();
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