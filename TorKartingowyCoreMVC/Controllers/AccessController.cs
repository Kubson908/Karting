using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using TorKartingowyCoreMVC.Models;
using TorKartingowyCoreMVC.Data;

namespace TorKartingowyCoreMVC.Controllers
{
    public class AccessController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AccessController(ApplicationDbContext db)
        {
            _db = db;
        }

        //GET
        public IActionResult Register()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Klient obj)
        {
            if (ModelState.IsValid)
            {
                _db.Klienci.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Dodano klienta";
                return RedirectToAction("Login");
            }
            return View(obj);
        }

        //GET
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if(claimUser.Identity != null && 
                claimUser.Identity.IsAuthenticated &&
                @User.Claims.FirstOrDefault(c => c.Type == "Role").Value == "Klient")
                return RedirectToAction("Index", "Home");

            return View();
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> Login(VMLogin modelLogin)
        {
            var klientFromDb = _db.Klienci.Find(modelLogin.Numer);
            if (klientFromDb != null && 
                klientFromDb.Numer == modelLogin.Numer && 
                klientFromDb.Haslo == modelLogin.Haslo)
            {
                modelLogin.Email = klientFromDb.Email;
                modelLogin.Imie = klientFromDb.Imie;
                modelLogin.Nazwisko = klientFromDb.Nazwisko;
                modelLogin.Telefon = klientFromDb.Telefon;
                modelLogin.Szkolenie = klientFromDb.Szkolenie;
                modelLogin.Rekord = klientFromDb.Rekord;
                ViewData["UserName"] = modelLogin.Imie;
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, modelLogin.Imie),
                    new Claim("Numer", klientFromDb.Numer.ToString()),
                    new Claim("Role", "Klient")
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = modelLogin.KeepLoggedIn
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);

                return RedirectToAction("Index", "Home");
            }


            ViewData["ValidateMessage"] = "Nie znaleziono użytkownika";
            return View();
        }
    }
}
