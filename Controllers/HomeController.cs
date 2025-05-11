using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace MyWebApp.Controllers
{
    public class HomeController : Controller
    {
        // Temporary storage for announcements
        private static List<(string Title, string Message)> _announcements = new();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Announcements()
        {
            // Show all announcements to the users
            return View(_announcements);
        }

        public IActionResult Discussions()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }

        // Admin login page (GET)
        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View();
        }

        // Admin login page (POST)
        [HttpPost]
    public IActionResult AdminLogin(string username, string password)
    {
        if (username == "Sarthak" && password == "sarthak123")
        {
            HttpContext.Session.SetString("IsAdmin", "true");
            return RedirectToAction("AddAnnouncement");
        }

        ViewBag.Error = "Invalid credentials.";
        return View();
    }

    public IActionResult AddAnnouncement()
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToAction("AdminLogin");

        return View();
    }

    [HttpPost]
    public IActionResult AddAnnouncement(string title, string message)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToAction("AdminLogin");

        _announcements.Add((title, message));
        return RedirectToAction("Announcements");
    }
    }
}