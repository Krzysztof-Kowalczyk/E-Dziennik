using System.Collections.Generic;
using System.Linq;
using Models.Interfaces;
using Models.Models;

namespace Repositories.Repositories
{
    public class LogRepository : ILogRepository
    {
        EDziennikContext db = new EDziennikContext();
        public List<Log> GetAll()
        {
            return db.Logs.ToList();
        }

        public Log FindById(int id)
        {
            return db.Logs.SingleOrDefault(a => a.Id == id);
        }

        public void Insert(Log item)
        {
            db.Logs.Add(item);
        }

        public void Update(Log item)
        {
            var log = db.Logs.Single(a => a.Id == item.Id);
            log.Action = item.Action;
            log.Date = item.Date;
            log.What = item.What;
            log.Who = item.Who;
            log.Ip = item.Ip;
        }

        public void Delete(int id)
        {
            var log = db.Logs.Single(a => a.Id == id);
            db.Logs.Remove(log);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}