using System.Collections.Generic;
using System.Linq;
using Models.Interfaces;
using Models.Models;
using System;

namespace Repositories.Repositories
{
    public class ClassroomRepository : IClassroomRepository, IDisposable
    {
        EDziennikContext db = new EDziennikContext();
        public IQueryable<Classroom> GetAll()
        {
            return db.Classrooms.AsNoTracking();
        }

        public IQueryable<Classroom> GetPage(int? page = 1, int? pageSize = 10)
        {
            var items = db.Classrooms.OrderByDescending(o=>o.Id).Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

            return items;
        } 

        public Classroom FindById(int id)
        {
            return db.Classrooms.SingleOrDefault(a => a.Id == id);
        }

        public void Insert(Classroom item)
        {
            db.Classrooms.Add(item);
        }

        public void Update(Classroom item)
        {
            var classRoom = db.Classrooms.Single(a => a.Id == item.Id);
            classRoom.Id = item.Id;
            classRoom.Name = item.Name;
            classRoom.Subjects = item.Subjects;
        }

        public void Delete(int id)
        {
            var classRoom = db.Classrooms.Single(a => a.Id == id);
            db.Classrooms.Remove(classRoom);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public IQueryable<Subject> GetSubjects(int classroomId)
        {
            return db.Subjects.Where(a => a.ClassroomId == classroomId);
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