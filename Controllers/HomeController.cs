using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.Models;

namespace MyWebApp.Controllers;

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
    /* announcements ma admin login page(main curly bracket vitra huncha ki nai recheck garnacha*/
        [HttpGet]
    public IActionResult AdminLogin()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AdminLogin(string username, string password)
    {
        if (username == "admin" && password == "password")
        {
            TempData["IsAdmin"] = true;
            return RedirectToAction("AddAnnouncement");
        }

        ViewBag.Error = "Invalid credentials.";
        return View();
    }
    /* announcements haru add garna lai form banako(eslai pani main bracket bahira ki vitra recheck garne)*/
    public IActionResult AddAnnouncement()
    {
        if (TempData["IsAdmin"] == null)
            return RedirectToAction("AdminLogin");

        return View();
    }

    [HttpPost]
    public IActionResult AddAnnouncement(string title, string message)
    {
        if (TempData["IsAdmin"] == null)
            return RedirectToAction("AdminLogin");

        _announcements.Add((title, message));
        return RedirectToAction("Announcements");
    }

}
