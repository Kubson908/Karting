using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using TorKartingowyCoreMVC.Models;
using TorKartingowyCoreMVC.Data;
using System.Text;
using System.Security.Cryptography;

namespace TorKartingowyCoreMVC.Controllers
{
    public class PracownikAccessController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PracownikAccessController(ApplicationDbContext db)
        {
            _db = db;
        }

        string hashPassword(string password)
        {
            var sha = SHA256.Create();
            var asByteArray = Encoding.Default.GetBytes(password);
            var hashedPassword = sha.ComputeHash(asByteArray);
            return Convert.ToBase64String(hashedPassword);
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
            modelLogin.Haslo = hashPassword(modelLogin.Haslo);
            if (PracownikFromDb != null &&
                PracownikFromDb.Id == modelLogin.Id &&
                modelLogin.Haslo.Equals(PracownikFromDb.Haslo))
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
