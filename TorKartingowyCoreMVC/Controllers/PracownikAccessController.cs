using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using TorKartingowyCoreMVC.Models;
using TorKartingowyCoreMVC.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TorKartingowyCoreMVC.Controllers
{
    public class PracownikAccessController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PracownikAccessController(ApplicationDbContext db)
        {
            _db = db;
        }

        //GET
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity != null && 
                claimUser.Identity.IsAuthenticated && 
                User.Claims.FirstOrDefault(c => c.Type == "Role").Value != "Klient")
                return RedirectToAction("Index", User.Claims.FirstOrDefault(c => c.Type == "Role").Value);

            return View();
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> Login(VMPracownik modelLogin)
        {
            var PracownikFromDb = _db.Pracownicy.Find(modelLogin.Id);
            if (PracownikFromDb != null &&
                PracownikFromDb.Id == modelLogin.Id &&
                PracownikFromDb.Haslo == modelLogin.Haslo)
            {
                modelLogin.Email = PracownikFromDb.Email;
                modelLogin.Haslo = PracownikFromDb.Haslo;
                modelLogin.Imie = PracownikFromDb.Imie;
                modelLogin.Nazwisko = PracownikFromDb.Nazwisko;
                modelLogin.Stanowisko = PracownikFromDb.Stanowisko;
                modelLogin.Telefon = PracownikFromDb.Telefon;
                modelLogin.Pensja = PracownikFromDb.Pensja;
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, modelLogin.Imie),
                    new Claim("Numer", PracownikFromDb.Id.ToString()),
                    new Claim("Role", modelLogin.Stanowisko)
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
                return RedirectToAction("Index", modelLogin.Stanowisko);
            }


            ViewData["ValidateMessage"] = "Nie znaleziono użytkownika";
            return View();
        }
    }
}
