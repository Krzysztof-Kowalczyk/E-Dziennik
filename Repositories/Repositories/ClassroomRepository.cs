using System;
using System.Linq;
using Models.Interfaces;
using Models.Models;

namespace Repositories.Repositories
{
    public class ClassroomRepository : IClassroomRepository
    {
        private readonly EDziennikContext _db = new EDziennikContext();

        public IQueryable<Classroom> GetAll()
        {
            return _db.Classrooms.AsNoTracking();
        }

        public IQueryable<Classroom> GetPage(int? page = 1, int? pageSize = 10)
        {
            var items = _db.Classrooms.OrderByDescending(o=>o.Id).Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

            return items;
        } 

        public Classroom FindById(int id)
        {
            return _db.Classrooms.SingleOrDefault(a => a.Id == id);
        }

        public void Insert(Classroom item)
        {
            _db.Classrooms.Add(item);
        }

        public void Update(Classroom item)
        {
            var classRoom = _db.Classrooms.Single(a => a.Id == item.Id);
            classRoom.Id = item.Id;
            classRoom.Name = item.Name;
            classRoom.Subjects = item.Subjects;
        }

        public void Delete(int id)
        {
            var classRoom = _db.Classrooms.Single(a => a.Id == id);
            _db.Classrooms.Remove(classRoom);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public IQueryable<Subject> GetSubjects(int classroomId)
        {
            return _db.Subjects.Where(a => a.ClassroomId == classroomId);
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