using System;
using System.Linq;
using Models.Interfaces;
using Models.Models;

namespace Repositories.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        readonly EDziennikContext _db = new EDziennikContext();
        public IQueryable<Subject> GetAll()
        {
            return _db.Subjects.AsNoTracking();
        }

        public IQueryable<Subject> GetPage(int? page = 1, int? pageSize = 10)
        {
            var items = _db.Subjects.OrderByDescending(o => o.Id).Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

            return items;
        } 

        public Subject FindById(int id)
        {
            return _db.Subjects.SingleOrDefault(a => a.Id == id);
        }

        public void Insert(Subject item)
        {
            _db.Subjects.Add(item);
        }

        public void Update(Subject item)
        {
            var subject = _db.Subjects.Single(a => a.Id == item.Id);
            subject.ClasssId = item.Id;
            subject.ClassroomId = item.ClassroomId;
            subject.Id = item.Id;
            subject.Name = item.Name;
            subject.TeacherId = item.TeacherId;
        }

        public void Delete(int id)
        {
            var subject = _db.Subjects.Single(a => a.Id == id);
            _db.Subjects.Remove(subject);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public IQueryable<Subject> FindByClassroomAndDate(int classroomId, int day, int hour)
        {
            return _db.Subjects.Where(a => a.ClassroomId == classroomId 
                                      && a.Day == ((SchoolDay)day) && a.Hour == hour);
        }

        public IQueryable<Subject> FindByClassroomAndDay(int classroomId, int day)
        {
           return _db.Subjects.Where(a => a.ClassroomId == classroomId && a.Day == ((SchoolDay)day));
        }

        public IQueryable<Subject> FindByClassroomId(int classroomId)
        {
            return _db.Subjects.Where(a => a.ClassroomId == classroomId);
        }

        public IQueryable<Subject> FindByClassId(int classId)
        {
            var subjects = _db.Subjects.Where(a => a.ClasssId == classId);
            
            return subjects;
        }

        public IQueryable<Subject> FindByTeacherId(string teacherId)
        {
            var subjects = _db.Subjects.Where(a => a.TeacherId == teacherId);

            return subjects;
        }

        public IQueryable<Subject> FindByStudentId(string studentId)
        {
            var classId = _db.Students.Single(a => a.Id == studentId).ClasssId;
            var subjects = _db.Subjects.Where(a => a.ClasssId == classId);

            return subjects;
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