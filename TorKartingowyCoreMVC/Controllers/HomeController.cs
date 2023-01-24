using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TorKartingowyCoreMVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TorKartingowyCoreMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity != null &&
                claimUser.Identity.IsAuthenticated &&
                @User.Claims.FirstOrDefault(c => c.Type == "Role").Value != "Klient")
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == "Role").Value;
                if (role == "Sprzętowiec") role = "Sprzetowiec";
                return RedirectToAction("Index", role);
            }
                

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Kontakt()
        {
            return View();
        }
    }
}