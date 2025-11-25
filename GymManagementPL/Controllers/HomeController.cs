using GymManagementBLL.BusinessServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnalyticService _analtyicService;

        public HomeController(IAnalyticService analtyicService)
        {
            _analtyicService = analtyicService;
        }
        //BaseUrl/Home/Index
        public ActionResult Index()
        {
            var data = _analtyicService.GetHomeAnalyticsViewModel();
            return View(data);
        }
    }
}
