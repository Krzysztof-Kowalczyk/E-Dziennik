using System.Collections.Generic;
using System.Linq;
using Models.Interfaces;
using Models.Models;
using System;

namespace Repositories.Repositories
{
    public class LogRepository : ILogRepository, IDisposable 
    {
        EDziennikContext db = new EDziennikContext();
        public IQueryable<Log> GetAll()
        {
            return db.Logs.AsNoTracking();
        }

        public IQueryable<Log> GetPage(int? page = 1, int? pageSize = 10)
        {
            var items = db.Logs.OrderByDescending(o => o.Id).Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

            return items;
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

         private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}