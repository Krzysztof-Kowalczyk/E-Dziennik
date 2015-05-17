using System;
using Models.Models;
using Repositories.Repositories;

namespace edziennik.Resources
{
    public static class Logs
    {
        private static LogRepository LogRepo { get; set; }

        public static void SaveLog(string action, string who,string what, string whatId)
        {
            var log = new Log
            {
                Action = action,
                Date = DateTime.Now,
                What = what,
                Who = who,
                WhatId = whatId
            };

           LogRepo = new LogRepository();
           LogRepo.Insert(log);  
           LogRepo.Save();
        }
    }
}