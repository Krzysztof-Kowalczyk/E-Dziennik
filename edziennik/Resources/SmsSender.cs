using System;
using edziennik.Models.ViewModels;
using Twilio;

namespace edziennik.Resources
{
    public static class SmsSender
    {
        const string AccountSid = "AC16b640bc801752" + "db415264631985ba2c";
        const string AuthToken = "c21b6fee75c30698571" + "87eaf7454fba4";
        public static bool SendSms(MarkCreateViewModel markVm, string number)
        {
            try
            {
                string teacher = ConstantStrings.TeacherRepo.FindById(markVm.TeacherId).FullName;
                string student = ConstantStrings.StudentRepo.FindById(markVm.StudentId).FullName;
                string subject = ConstantStrings.SubjectRepo.FindById(markVm.SubjectId).Name;
                string body = String.Format("Witaj ! Uczeń {0} otrzymał właśnie od nauczyciela {1}  z przedmiotu {2} ocenę {3} za {4}",
                                                                                       student, teacher, subject, markVm.Value, markVm.Description);
                var twilio = new TwilioRestClient(AccountSid, AuthToken);
                twilio.SendMessage("+19803656518", "+48694307553", body);

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public static bool SendSms(MarkCreateForSubjectViewModel markVm, string number)
        {
            try
            {
                string teacher = ConstantStrings.TeacherRepo.FindById(markVm.TeacherId).FullName;
                string student = ConstantStrings.StudentRepo.FindById(markVm.StudentId).FullName;
                string subject = ConstantStrings.SubjectRepo.FindById(markVm.SubjectId).Name;
                string body = String.Format("Witaj ! Uczeń {0} otrzymał właśnie od nauczyciela {1}  z przedmiotu {2} ocenę {3} za {4}",
                                                                                       student, teacher, subject, markVm.Value, markVm.Description);
                var twilio = new TwilioRestClient(AccountSid, AuthToken);
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