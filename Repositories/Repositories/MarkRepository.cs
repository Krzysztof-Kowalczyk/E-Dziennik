using System.Collections.Generic;
using System.Linq;
using Models.Interfaces;
using Models.Models;
using System;

namespace Repositories.Repositories
{
    public class MarkRepository : IMarkRepository, IDisposable
    {
        EDziennikContext db = new EDziennikContext();
        public List<Mark> GetAll()
        {
            return db.Marks.ToList();
        }

        public List<Mark> GetPage(int? page = 1, int? pageSize = 10)
        {
            var items = db.Marks.OrderByDescending(o => o.Id).Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();

            return items;
        } 

        public Mark FindById(int id)
        {
            return db.Marks.SingleOrDefault(a => a.Id == id);
        }

        public void Insert(Mark item)
        {
            db.Marks.Add(item);
        }

        public void Update(Mark item)
        {
            var mark = db.Marks.Single(a => a.Id == item.Id);
            mark.Value = item.Value;
            mark.Description = item.Description;
            mark.StudentId = item.StudentId;
            mark.SubjectId = item.SubjectId;
        }

        public void Delete(int id)
        {
            var mark = db.Marks.Single(a => a.Id == id);
            db.Marks.Remove(mark);
        }

        public void Save()
        {
            db.SaveChanges();
        }

       public List<Mark> FindByStudentIdAndSubjectId(string studentId, int subjectId)
        {
            var marks = db.Marks.Where(a => a.StudentId == studentId && a.SubjectId == subjectId).ToList();

            return marks;
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