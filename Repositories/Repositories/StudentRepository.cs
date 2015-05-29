using System.Collections.Generic;
using System.Linq;
using Models.Interfaces;
using Models.Models;
using System;

namespace Repositories.Repositories
{
    public class StudentRepository : IStudentRepository, IDisposable
    {
        EDziennikContext db = new EDziennikContext();
        public IQueryable<Student> GetAll()
        {
            return db.Students.AsNoTracking();
        }

        public IQueryable<Student> GetPage(int? page = 1, int? pageSize = 10)
        {
            var items = db.Students.OrderByDescending(o => o.Id).Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

            return items;
        } 

        public Student FindById(string id)
        {
            return db.Students.SingleOrDefault(a => a.Id == id);           
        }

        public IQueryable<Student> FindBySurname(string surname)
        {
            return db.Students.Where(a => a.Surname == surname.ToLower());
        }

        public void Insert(Student item)
        {
            db.Students.Add(item);
        }

        public void Update(Student item)
        {
            var student = db.Students.Single(a => a.Id == item.Id);
            student.ClasssId = item.ClasssId;
            student.Marks = item.Marks;
            student.FirstName = item.FirstName;
            student.SecondName = item.SecondName;
            student.Surname = item.Surname;
        }

        public void Delete(string id)
        {
            var student = db.Students.Single(a => a.Id == id);
            var marks = student.Marks.ToList();
            db.Marks.RemoveRange(marks);
            db.Students.Remove(student);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public Student FindByMarkId(int markId)
        {
            var mark = db.Marks.Single(a => a.Id == markId);

            return db.Students.Single(a => a.Id == mark.StudentId);
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