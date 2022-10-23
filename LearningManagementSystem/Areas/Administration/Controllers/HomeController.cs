using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
