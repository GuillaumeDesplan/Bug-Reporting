using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tickets_bug_BSD2023_2025.Data;
using tickets_bug_BSD2023_2025.Models;

namespace tickets_bug_BSD2023_2025.Controllers
{
    public class HomeController : Controller
    {
        private string SessionUserName = "SessionUserName";
        private string SessionUserId = "SessionUserId";
        private string SessionRole = "SessionRole";

        private readonly tickets_bug_BSD2023_2025Context _context;

        public HomeController(tickets_bug_BSD2023_2025Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //HttpContext.Session.SetString("cléA", "a@a.com");
            //HttpContext.Session.SetInt32("cléB", 42);

            return View();
        }

        public IActionResult Privacy()
        {
            //HttpContext.Session.GetString("cléA");
            //HttpContext.Session.GetInt32("cléB");

            return View();
        }

        public IActionResult Signin()
        {
            return View(new Home());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signin([Bind("Email","Password")]Home home)
        {
            if (ModelState.IsValid)
            {
                home.Password = BCrypt.Net.BCrypt.HashPassword(home.Password);
                home.Role = "Client";

                _context.Add(home);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(home);

        }

        public IActionResult Login()
        {
            return View(new Home());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email,Password")] Home home)
        {
            if (ModelState.IsValid)
            {

                if (home.Email == null || home.Password == null)
                {
                    return View(home);
                }

                var homeBdd = await _context.Home
                    .FirstOrDefaultAsync(u => u.Email == home.Email);

                if (homeBdd == null)
                {
                    return View(home);
                }

                if (!BCrypt.Net.BCrypt.Verify(home.Password, homeBdd.Password))
                {
                    return View(home);
                }

                HttpContext.Session.SetString(SessionUserName, homeBdd.Email);
                HttpContext.Session.SetString(SessionRole, homeBdd.Role);

                return RedirectToAction(nameof(Index));
            }
            return View(home);
        }
        public IActionResult Logout()
        {
            // Clear the session
            HttpContext.Session.Clear();

            // Redirect to the Login page
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
