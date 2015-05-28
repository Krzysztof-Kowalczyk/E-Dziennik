using System;
using edziennik.Models;
using edziennik.Models.ViewModels;
using Twilio;

namespace edziennik.Resources
{
    public static class SmsSender
    {
        public static bool SendSms(MarkCreateViewModel markVm, string number)
        {
            try
            {
                const string accountSid = "AC16b640bc801752db415264631985ba2c";
                const string authToken = "d1c5e1859d1ce8c840a771552807c1fb";
                string teacher = ConstantStrings.TeacherRepo.FindById(markVm.TeacherId).FullName;
                string student = ConstantStrings.StudentRepo.FindById(markVm.StudentId).FullName;
                string subject = ConstantStrings.SubjectRepo.FindById(markVm.SubjectId).Name;
                string body = String.Format("Witaj ! Uczeń {0} otrzymał właśnie od nauczyciela {1}  z przedmiotu {2} ocenę {3} za {4}",
                                                                                       student, teacher, subject, markVm.Value, markVm.Description);
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