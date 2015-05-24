using System.Collections.Generic;
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
        public const string ClassCreateError = "Nie można stworzyć klasy, gdyż nie istnieje żaden nauczyciel";
        public const string SubjectCreateNoTeachersError = "Nie można stworzyć przedmiotu, gdyż nie istnieje żaden nauczyciel";
        public const string SubjectCreateNoClassesError = "Nie można stworzyć przedmiotu, gdyż nie istnieje żadena klasa";
        public const string SubjectCreateNoClassroomsError = "Nie można stworzyć przedmiotu, gdyż nie istnieje żadna sala";
        public const int MaxClassStudentCount = 30;

        public static SubjectRepository SubjectRepo
        {
            get { return new SubjectRepository(); }
        }

        public static StudentRepository StudentRepo 
        {
            get { return new StudentRepository(); }
        }

        public static TeacherRepository TeacherRepo 
        {
            get { return new TeacherRepository(); }
        }

        public static ClassroomRepository ClassroomRepo 
        {
            get { return new ClassroomRepository(); }
        }

        public static ClasssRepository ClassRepo 
        {
            get { return new ClasssRepository(); }
        }

        public static List<SelectListItem> getStudentsSL()
        {
            var students = StudentRepo.GetAll().Select(c => new SelectListItem
            {
                Value = c.Id,
                Text = c.FirstName + " " + c.Surname

            }).ToList();

            return students;
        }

        public static List<SelectListItem> getStudentSubjectsSL(int classId, string teacherId)
        {
            var subjects =
                SubjectRepo.FindByClassId(classId).Where(a => a.TeacherId == teacherId).Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();
                                                

            return subjects;
            
        }

        public static List<SelectListItem> getTeachersSL()
        {
            var teachers = TeacherRepo.GetAll().Select(c => new SelectListItem
            {
                Value = c.Id,
                Text = c.FirstName + " " + c.Surname

            }).ToList();

            return teachers;
        }

        public static List<SelectListItem> getClassesSL()
        {
            var classes = ClassRepo.GetAll().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text=c.Name
            }).ToList();

            return classes;
        }

        public static List<SelectListItem> getClassroomsSL()
        {
            //var classrooms = new SelectList(classroomRepo.GetAll(), "Id", "Name"); ;
            var classrooms = ClassroomRepo.GetAll().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            return classrooms;
        }

        public static List<SelectListItem> getSchoolHoursSL()
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

        public static List<SelectListItem> getSchoolDaysSL()
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