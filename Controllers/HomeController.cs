using Microsoft.AspNetCore.Mvc;

namespace FireDataWebService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
