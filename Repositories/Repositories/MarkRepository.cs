using System.Collections.Generic;
using System.Linq;
using Models.Interfaces;
using Models.Models;

namespace Repositories.Repositories
{
    public class MarkRepository : IMarkRepository
    {
        EDziennikContext db = new EDziennikContext();
        public List<Mark> GetAll()
        {
            return db.Marks.ToList();
        }

        public Mark FindById(int id)
        {
            return db.Marks.Single(a => a.Id == id);
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
    }
}