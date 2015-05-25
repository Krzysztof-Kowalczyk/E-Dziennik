using System.Web.Mvc;

namespace edziennik.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(Duration = 30)]
        public ActionResult About()
        {
            ViewBag.Message = "Opis aplikacji";

            return View();
        }

        [OutputCache(Duration = 30)]
        public ActionResult Contact()
        {
            ViewBag.Message = "Dane kontaktowe";

            return View();
        }


    }
}