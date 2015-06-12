using System;
using System.Linq;
using Models.Interfaces;
using Models.Models;

namespace Repositories.Repositories
{
    public class MarkRepository : IMarkRepository
    {
        readonly EDziennikContext _db = new EDziennikContext();
        public IQueryable<Mark> GetAll()
        {
            return _db.Marks.AsNoTracking();
        }

        public IQueryable<Mark> GetPage(int? page = 1, int? pageSize = 10)
        {
            var items = _db.Marks.OrderByDescending(o => o.Id).Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

            return items;
        } 

        public Mark FindById(int id)
        {
            return _db.Marks.SingleOrDefault(a => a.Id == id);
        }

        public void Insert(Mark item)
        {
            _db.Marks.Add(item);
        }

        public void Update(Mark item)
        {
            var mark = _db.Marks.Single(a => a.Id == item.Id);
            mark.Value = item.Value;
            mark.Description = item.Description;
            mark.StudentId = item.StudentId;
            mark.SubjectId = item.SubjectId;
        }

        public void Delete(int id)
        {
            var mark = _db.Marks.Single(a => a.Id == id);
            _db.Marks.Remove(mark);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public IQueryable<Mark> FindByStudentIdAndSubjectId(string studentId, int subjectId)
        {
            var marks = _db.Marks.Where(a => a.StudentId == studentId && a.SubjectId == subjectId);

            return marks;
        }

        public IQueryable<Mark> FindByStudentId(string studentId)
        {
            var marks = _db.Marks.Where(a => a.StudentId == studentId);

            return marks;
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