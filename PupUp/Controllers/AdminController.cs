using Microsoft.AspNetCore.Mvc;

namespace PupUp.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
