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
        private static SubjectRepository subjectRepo = new SubjectRepository ();
        private static TeacherRepository teacherRepo = new TeacherRepository();
        private static ClassroomRepository classroomRepo = new ClassroomRepository();
        private static ClasssRepository classRepo = new ClasssRepository();

        public static List<SelectListItem> getTeachersSL()
        {
            var teachers = teacherRepo.GetAll().Select(c => new SelectListItem
            {
                Value = c.Id,
                Text = c.FirstName + " " + c.Surname

            }).ToList();

            return teachers;

        }

    }
}