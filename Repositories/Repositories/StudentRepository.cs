using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models.Interfaces;
using Models.Models;

namespace Repositories.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        EDziennikContext db = new EDziennikContext();
        public List<Student> GetAll()
        {
            return db.Students.ToList();
        }

        public Student FindById(string id)
        {
            return db.Students.Single(a => a.Id == id);           
        }

        public List<Student> FindBySurname(string surname)
        {
            return db.Students.Where(a => a.Surname == surname.ToLower()).ToList();
        }

        public void Insert(Student item)
        {
            db.Students.Add(item);
        }

        public void Update(Student item)
        {
            var student = db.Students.Single(a => a.Id == item.Id);
            student.ClassId = item.ClassId;
            student.Marks = item.Marks;
            student.FirstName = item.FirstName;
            student.SecondName = item.SecondName;
            student.Surname = item.Surname;
        }

        public void Delete(string id)
        {
            var student = db.Students.Single(a => a.Id == id);
            var marks = student.Marks.ToList();
            db.Marks.RemoveRange(marks);
            db.Students.Remove(student);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public List<Mark> GetMarks(string studentId)
        {
            return db.Marks.Where(a => a.StudentId == studentId).ToList();
        }
    }
}