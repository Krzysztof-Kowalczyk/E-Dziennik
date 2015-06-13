using System;
using System.Linq;
using Models.Interfaces;
using Models.Models;

namespace Repositories.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        readonly EDziennikContext _db = new EDziennikContext();
        public IQueryable<Student> GetAll()
        {
            return _db.Students.AsNoTracking();
        }

        public IQueryable<Student> GetPage(int? page = 1, int? pageSize = 10)
        {
            var items = _db.Students.OrderByDescending(o => o.Id).Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

            return items;
        } 

        public Student FindById(string id)
        {
            return _db.Students.SingleOrDefault(a => a.Id == id);           
        }

        public IQueryable<Student> FindBySurname(string surname)
        {
            return _db.Students.Where(a => a.Surname == surname.ToLower());
        }

        public void Insert(Student item)
        {
            _db.Students.Add(item);
        }

        public void Update(Student item)
        {
            var student = _db.Students.Single(a => a.Id == item.Id);
            student.ClasssId = item.ClasssId;
            student.Marks = item.Marks;
            student.FirstName = item.FirstName;
            student.SecondName = item.SecondName;
            student.Surname = item.Surname;
        }

        public void Delete(string id)
        {
            var student = _db.Students.Single(a => a.Id == id);
            var marks = student.Marks.ToList();
            _db.Marks.RemoveRange(marks);
            _db.Students.Remove(student);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public Student FindByMarkId(int markId)
        {
            var mark = _db.Marks.Single(a => a.Id == markId);

            return _db.Students.Single(a => a.Id == mark.StudentId);
        }

        public IQueryable<Student> FindByClassId(int classId)
        {
            return _db.Students.Where(a => a.ClasssId == classId);
        }

        public IQueryable<Student> FindBySubjectId(int subjectId)
        {
            var classId = _db.Subjects.Single(a => a.Id == subjectId).ClasssId;
            
            return _db.Students.Where(a => a.ClasssId == classId);
        }

        public IQueryable<Student> FindByTutorId(string tutorId)
        {
            var classId = _db.Classes.Single(a => a.TeacherId ==tutorId).Id;

            return _db.Students.Where(a => a.ClasssId == classId);
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