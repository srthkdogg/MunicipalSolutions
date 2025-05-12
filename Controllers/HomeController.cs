using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace MyWebApp.Controllers
{
    public class HomeController : Controller
    {
        // Temporary storage for announcements
        private static int _announcementIdCounter = 1;
        private static List<Announcement> _announcements = new();
        private readonly ILogger<HomeController> _logger;
        private static List<DiscussionPost> _discussions = new();

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
        {    // announcements for viewers and admin
            ViewBag.IsAdmin = HttpContext.Session.GetString("IsAdmin") == "true";
            return View(_announcements);
        }

        [HttpGet]
        public IActionResult Discussions()
        {
            return View(_discussions.OrderByDescending(d => d.PostedAt).ToList());
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
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("IsAdmin"); // Remove admin session
            return RedirectToAction("Index");      // Redirect to home page or wherever you want
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

            _announcements.Add(new Announcement
            {
                Id = _announcementIdCounter++,
                Title = title,
                Message = message
            });

            return RedirectToAction("Announcements");
        }

        [HttpPost]
        public IActionResult DeleteAnnouncement(int id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return Unauthorized(); // Only admins can delete
            }

            var item = _announcements.FirstOrDefault(a => a.Id == id);
            if (item != null)
            {
                _announcements.Remove(item);
            }

            return RedirectToAction("Announcements");
        }
        [HttpGet]
        public IActionResult AddDiscussion() //for discussion
        {
        // Make sure only logged-in users can access the form
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
                return RedirectToAction("Login", "Account"); // Redirect to login page

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddDiscussion(DiscussionPostViewModel model)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
                return RedirectToAction("Login", "Account");

            string imagePath = null;

    // Handle image upload
            if (model.Image != null)
            {
                var fileName = Path.GetFileName(model.Image.FileName);
                var filePath = Path.Combine("wwwroot/uploads", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }

                imagePath = "/uploads/" + fileName;
            }

            _discussions.Add(new DiscussionPost
            {
                Title = model.Title,
                Message = model.Message,
                ImagePath = imagePath,
                UserName = HttpContext.Session.GetString("Username"),
                PostedAt = DateTime.Now
            });

            return RedirectToAction("Discussions");
        }
    }
}