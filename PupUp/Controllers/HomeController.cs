using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PupUp.Data;
using PupUp.Helpers.Extensions;
using PupUp.Models;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace PupUp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PupUpDbContext m_dbContext;
        public HomeController(ILogger<HomeController> logger, PupUpDbContext dbContext)
        {
            _logger = logger;
            m_dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Services()
        {
            return View();
        }
        [Authorize]
        public IActionResult Profil()
        {
            ViewData["Dogs"] = m_dbContext.Dogs.Where(d => d.UserId == User.Claims.GetClaim(ClaimTypes.NameIdentifier)).ToList();
            ViewData["Quests"] = m_dbContext.Quests.ToList();
            ViewData["Badges"] = m_dbContext.Badges.ToList();
            ViewData["UserQuests"] = m_dbContext.UserQuests.Where(d => d.UserId == User.Claims.GetClaim(ClaimTypes.NameIdentifier)).ToList();
            ViewData["UserBadges"] = m_dbContext.UserBadges.Where(d => d.UserId == User.Claims.GetClaim(ClaimTypes.NameIdentifier)).ToList();
            ViewData["Coin"] = m_dbContext.Points.FirstOrDefault(d => d.UserId == User.Claims.GetClaim(ClaimTypes.NameIdentifier))?.Coin ?? 0;
            return View();
        }
        public IActionResult Denied()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
