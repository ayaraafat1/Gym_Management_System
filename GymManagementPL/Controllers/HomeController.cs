using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public RedirectResult Redirect()
        {
            return Redirect("https://www.facebook.com/");
        }
    }
}
