using Models.Models;
using Repositories.Repositories;

namespace edziennik.Resources
{
    public static class Logs
    {
        private static LogRepository LogRepository 
        {
            get {return new LogRepository(); }
        }

        public static void SaveLog()
        {
           var log = new Log();
           LogRepository.Insert(log);  
        }
    }
}