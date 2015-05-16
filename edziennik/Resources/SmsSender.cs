using System;
using Twilio;

namespace edziennik.Resources
{
    public static class SmsSender
    {
        public static bool SendSms(string studentId, string teacherId, int subjectId, double value, string description)
        {
            try
            {
                const string accountSid = "AC16b640bc801752db415264631985ba2c";
                const string authToken = "d1c5e1859d1ce8c840a771552807c1fb";
                string teacher = ConstantStrings.TeacherRepo.FindById(teacherId).FullName;
                string student = ConstantStrings.StudentRepo.FindById(studentId).FullName;
                string subject = ConstantStrings.SubjectRepo.FindById(subjectId).Name;
                string body = String.Format("Witaj ! Uczeń {0} otrzymał właśnie od nauczyciela {1}  z przedmiotu {2} ocenę {3} za {4}",
                                                                                       student, teacher, subject, value, description);
                var twilio = new TwilioRestClient(accountSid, authToken);
                twilio.SendMessage("+19803656518", "+48694307553", body);
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}