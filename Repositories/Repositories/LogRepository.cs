using System;
using System.Linq;
using Models.Interfaces;
using Models.Models;

namespace Repositories.Repositories
{
    public class LogRepository : ILogRepository 
    {
        readonly EDziennikContext _db = new EDziennikContext();
        public IQueryable<Log> GetAll()
        {
            return _db.Logs.AsNoTracking();
        }

        public IQueryable<Log> GetPage(int? page = 1, int? pageSize = 10)
        {
            var items = _db.Logs.OrderByDescending(o => o.Id).Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

            return items;
        } 

        public Log FindById(int id)
        {
            return _db.Logs.SingleOrDefault(a => a.Id == id);
        }

        public void Insert(Log item)
        {
            _db.Logs.Add(item);
        }

        public void Update(Log item)
        {
            var log = _db.Logs.Single(a => a.Id == item.Id);
            log.Action = item.Action;
            log.Date = item.Date;
            log.What = item.What;
            log.Who = item.Who;
            log.Ip = item.Ip;
        }

        public void Delete(int id)
        {
            var log = _db.Logs.Single(a => a.Id == id);
            _db.Logs.Remove(log);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

         private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}