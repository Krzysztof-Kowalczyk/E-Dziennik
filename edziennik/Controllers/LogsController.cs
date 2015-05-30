using Microsoft.AspNet.Identity;
using Models.Models;
using Repositories.Repositories;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using edziennik.Models.ViewModels;
using PagedList;
using System;

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
        public ActionResult Index(int? page, string sortOrder)
        {
            int currentPage = page ?? 1;
            var items = SortItems(sortOrder);

            var logs = items.ToList().Select(a => new LogListItemViewModel
                {
                    Id = a.Id,
                    Action = a.Action,
                    What = a.What,
                    Who = userManager.FindById(a.Who).UserName,
                    Date = a.Date
                }).ToPagedList(currentPage, 10);
            
            if (Request.IsAjaxRequest())
            {
                return PartialView("_LogList", logs);
            }

            return View(logs);
        }

        [NonAction]
        private IQueryable<Log> SortItems(string sortOrder)
        {
            var items = LogRepository.GetAll();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.IdSort = String.IsNullOrEmpty(sortOrder) ? "IdAsc" : "";
            ViewBag.DateSort = sortOrder == "Date" ? "DateAsc" : "Date";
            ViewBag.ActionSort = sortOrder == "ActionAsc" ? "Action" : "ActionAsc";
            ViewBag.WhatSort = sortOrder == "WhatAsc" ? "What" : "WhatAsc";
            ViewBag.WhoSort = sortOrder == "WhoAsc" ? "Who" : "WhoAsc";

            switch (sortOrder)
            {
                case "Date":
                    items = items.OrderByDescending(s => s.Date);
                    break;
                case "DateAsc":
                    items = items.OrderBy(s => s.Date);
                    break;
                case "Action":
                    items = items.OrderByDescending(s => s.Action);
                    break;
                case "ActionAsc":
                    items = items.OrderBy(s => s.Action);
                    break;
                case "What":
                    items = items.OrderByDescending(s => s.What);
                    break;
                case "WhatAsc":
                    items = items.OrderBy(s => s.What);
                    break;
                case "Who":
                    items = items.OrderByDescending(s => s.Who);
                    break;
                case "WhoAsc":
                    items = items.OrderBy(s => s.Who);
                    break;
                case "IdAsc":
                    items = items.OrderBy(s => s.Id);
                    break;
                default:    // id descending
                    items = items.OrderByDescending(s => s.Id);
                    break;
            }
            return items;
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
            Log log = LogRepository.FindById((int)id);
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
