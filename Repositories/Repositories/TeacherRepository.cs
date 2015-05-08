using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models.Interfaces;
using Models.Models;

namespace Repositories.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        EDziennikContext db = new EDziennikContext();
        public List<Teacher> GetAll()
        {
            return db.Teachers.ToList();
        }

        public Teacher FindById(string id)
        {
            return db.Teachers.Single(a => a.Id == id);
        }

        public List<Teacher> FindBySurname(string surname)
        {
            return db.Teachers.Where(a => a.Surname == surname.ToLower()).ToList();
        }

        public void Insert(Teacher item)
        {
            db.Teachers.Add(item);
        }

        public void Update(Teacher item)
        {
            var teacher= db.Teachers.Single(a => a.Id == item.Id);
            teacher.FirstName = item.FirstName;
            teacher.SecondName = item.SecondName;
            teacher.Surname = item.Surname;
            teacher.ClassList = item.ClassList;
            teacher.Pesel = item.Pesel;
        }

        public void Delete(string id)
        {
            var teacher = db.Teachers.Single(a => a.Id == id);
            db.Teachers.Remove(teacher);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public List<Classs> GetClasses(int teacherId)
        {
            var classes = from classs in db.Classes
                join subject in db.Subjects
                    on classs.Id equals subject.ClasssId
                select classs;

            return classes.ToList();
        }

        public List<Subject> GetSubjects(int teacherId)
        {
            return db.Subjects.Where(a => a.TeacherId == teacherId).ToList();
        }
    }
}