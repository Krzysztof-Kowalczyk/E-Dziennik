using Microsoft.AspNet.Identity;
using Models.Models;
using Repositories.Repositories;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using edziennik.Models.ViewModels;

namespace edziennik.Controllers
{
    [Authorize(Roles = "Admins")]
    public class LogsController : Controller
    {
        private readonly LogRepository LogRepository;
        private readonly ApplicationUserManager userManager;

        public LogsController(LogRepository logRepository, ApplicationUserManager _userManager)
        {
            LogRepository = logRepository;
            this.userManager = _userManager;
        }

        // GET: Logs
        public ActionResult Index()
        {
            var logs = LogRepository.GetAll().Select(a=>new LogListItemViewModel
                {
                    Id = a.Id,
                    Action = a.Action,
                    What = a.What,
                    Who = userManager.FindById(a.Who).UserName,
                    Date = a.Date
                });
            return View(logs);
        }

        // GET: Logs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Log log = LogRepository.FindById((int)id);

            if (log == null)
            {
                return HttpNotFound();
            }
            var logVm = new LogDetailsViewModel
            {
                Action = log.Action,
                Date = log.Date,
                What = log.What,
                WhatId = log.WhatId,
                Who = userManager.FindById(log.Who).UserName,
                Id = log.Id,
                Ip = log.Ip
            };
            return View(logVm);
        }

        // GET: Logs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Log log = LogRepository.FindById((int) id);
            if (log == null)
            {
                return HttpNotFound();
            }
            return View(log);
        }

        // POST: Logs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LogRepository.Delete(id);
            LogRepository.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            LogRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}
