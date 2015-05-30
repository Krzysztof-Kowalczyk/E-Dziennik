using System.Collections.Generic;
using System.Linq;
using Models.Interfaces;
using Models.Models;
using System;

namespace Repositories.Repositories
{
    public class SubjectRepository : ISubjectRepository, IDisposable
    {
        EDziennikContext db = new EDziennikContext();
        public IQueryable<Subject> GetAll()
        {
            return db.Subjects.AsNoTracking();
        }

        public IQueryable<Subject> GetPage(int? page = 1, int? pageSize = 10)
        {
            var items = db.Subjects.OrderByDescending(o => o.Id).Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

            return items;
        } 

        public Subject FindById(int id)
        {
            return db.Subjects.SingleOrDefault(a => a.Id == id);
        }

        public void Insert(Subject item)
        {
            db.Subjects.Add(item);
        }

        public void Update(Subject item)
        {
            var subject = db.Subjects.Single(a => a.Id == item.Id);
            subject.ClasssId = item.Id;
            subject.ClassroomId = item.ClassroomId;
            subject.Id = item.Id;
            subject.Name = item.Name;
            subject.TeacherId = item.TeacherId;
        }

        public void Delete(int id)
        {
            var subject = db.Subjects.Single(a => a.Id == id);
            db.Subjects.Remove(subject);
        }

        public void Save()
        {
            db.SaveChanges();
        }

         public IQueryable<Subject> FindByDate(int classroomId, int day)
        {
           return db.Subjects.Where(a => a.ClassroomId == classroomId && a.Day == ((SchoolDay)day));
        }

        public IQueryable<Subject> FindByClassId(int classId)
        {
            var subjects = db.Subjects.Where(a => a.ClasssId == classId);
            
            return subjects;
        }

        public IQueryable<Subject> FindByTeacherId(string teacherId)
        {
            var subjects = db.Subjects.Where(a => a.TeacherId == teacherId);

            return subjects;
        }

        public IQueryable<Subject> FindByStudentId(string studentId)
        {
            var classId = db.Students.Single(a => a.Id == studentId).ClasssId;
            var subjects = db.Subjects.Where(a => a.ClasssId == classId);

            return subjects;
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