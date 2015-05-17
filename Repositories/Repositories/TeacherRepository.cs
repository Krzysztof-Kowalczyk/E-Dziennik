﻿using System.Collections.Generic;
using System.Linq;
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
            return db.Teachers.SingleOrDefault(a => a.Id == id);
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
    }
}