﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Repositories.Repositories;

namespace edziennik.Resources
{
    public static class ConstantStrings
    {
        public const string DefaultUserAvatar = "~/Resources/Images/Users/defaultavatar.png";
        public const string UserAvatarsPath = "~/Resources/Images/Users/";
        public const string LogsPath = "~/Resources/Logs/";
        public const int MaxClassStudentCount = 30;
        public static SubjectRepository subjectRepo = new SubjectRepository();
        public static StudentRepository studentRepo = new StudentRepository();
        public static TeacherRepository teacherRepo = new TeacherRepository();
        public static ClassroomRepository classroomRepo = new ClassroomRepository();
        public static ClasssRepository classRepo = new ClasssRepository();

        public static List<SelectListItem> getStudentsSL()
        {
            var students = studentRepo.GetAll().Select(c => new SelectListItem
            {
                Value = c.Id,
                Text = c.FirstName + " " + c.Surname

            }).ToList();

            return students;
        }

        public static SelectList getStudentSubjectsSL(int classId, string teacherId)
        {
            var subjects = new SelectList(subjectRepo.FindByClassId
                                                           (classId).Where(a=>a.TeacherId==teacherId), "Id", "Name");

            return subjects;
            
        }

        public static int getClassStudentCount(int classId)
        {
            return classRepo.FindById(classId).Students.Count;
        }

        public static List<SelectListItem> getTeachersSL()
        {
            var teachers = teacherRepo.GetAll().Select(c => new SelectListItem
            {
                Value = c.Id,
                Text = c.FirstName + " " + c.Surname

            }).ToList();

            return teachers;
        }

        public static SelectList getClassesSL()
        {
            var classes = new SelectList(classRepo.GetAll(), "Id", "Name"); 

            return classes;
        }

        public static SelectList getClassroomsSL()
        {
            var classrooms = new SelectList(classroomRepo.GetAll(), "Id", "Name"); ;

            return classrooms;
        }

        public static List<SelectListItem> getSchoolHours()
        {
            var hours = new List<SelectListItem>()
            {
                new SelectListItem() {Text = "8:00 - 8:45", Value = "1"},
                new SelectListItem() {Text = "8:50 - 9:35", Value = "2"},
                new SelectListItem() {Text = "9:55 - 10:40", Value = "3"},
                new SelectListItem() {Text = "10:45 - 11:30", Value = "4"},
                new SelectListItem() {Text = "11:40 - 12:25", Value = "5"},
                new SelectListItem() {Text = "12:30 - 13:15", Value = "6"},
                new SelectListItem() {Text = "13:20 - 14:05", Value = "7"},
                new SelectListItem() {Text = "14:10 - 14:55", Value = "8"},
                new SelectListItem() {Text = "15:00 - 15:45", Value = "9"},
                new SelectListItem() {Text = "15:50 - 16:35", Value = "10"},
                new SelectListItem() {Text = "16:40 - 17:25", Value = "11"},
                new SelectListItem() {Text = "17:30 - 18:15", Value = "12"}
            };

            return hours;
        }

        public static List<SelectListItem> getSchoolDays()
        {
            var days = new List<SelectListItem>()
            {
                new SelectListItem() {Text = "Poniedziałek", Value = "1"},
                new SelectListItem() {Text = "Wtorek", Value = "2"},
                new SelectListItem() {Text = "Środa", Value = "3"},
                new SelectListItem() {Text = "Czwartek", Value = "4"},
                new SelectListItem() {Text = "Piatek", Value = "5"}
         
            };
            return days;
        }

        public static List<SelectListItem> getMarksSL()
        {
            var days = new List<SelectListItem>()
            {
                new SelectListItem() {Text = "Niedostateczny", Value = "1"},
                new SelectListItem() {Text = "Dopuszczający", Value = "2"},
                new SelectListItem() {Text = "Dostateczny", Value = "3"},
                new SelectListItem() {Text = "Dobry", Value = "4"},
                new SelectListItem() {Text = "Bardzo dobry", Value = "5"}
         
            };
            return days;
        }

    }
}