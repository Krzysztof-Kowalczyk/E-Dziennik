using System.Collections.Generic;
using System.Linq;
using Models.Interfaces;
using Models.Models;

namespace Repositories.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        EDziennikContext db = new EDziennikContext();
        public List<Subject> GetAll()
        {
            return db.Subjects.ToList();
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

        public List<Subject> FindByClassId(int classId)
        {
            var subjects = db.Subjects.Where(a => a.ClasssId == classId).ToList();
            
            return subjects;
        }

        public List<Subject> FindByTeacherId(string teacherId)
        {
            var subjects = db.Subjects.Where(a => a.TeacherId == teacherId).ToList();

            return subjects;
        }

        public List<Subject> FindByStudentId(string studentId)
        {
            var classId = db.Students.Single(a => a.Id == studentId).ClasssId;
            var subjects = db.Subjects.Where(a => a.ClasssId == classId).ToList();

            return subjects;
        }
    }
}