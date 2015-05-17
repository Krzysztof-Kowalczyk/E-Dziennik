using System.Collections.Generic;
using System.Linq;
using Models.Interfaces;
using Models.Models;

namespace Repositories.Repositories
{
    public class ClassroomRepository : IClassroomRepository
    {
        EDziennikContext db = new EDziennikContext();
        public List<Classroom> GetAll()
        {
            return db.Classrooms.ToList();
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

        public List<Subject> GetSubjects(int classroomId)
        {
            return db.Subjects.Where(a => a.ClassroomId == classroomId).ToList();
        }
    }
}