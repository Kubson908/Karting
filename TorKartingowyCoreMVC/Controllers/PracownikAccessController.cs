using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using TorKartingowyCoreMVC.Models;
using TorKartingowyCoreMVC.Data;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

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
                string stanowisko = modelLogin.Stanowisko;
                if (stanowisko == "Sprzętowiec") stanowisko = "Sprzetowiec";
                return RedirectToAction("Index", stanowisko);
            }
            ViewData["ValidateMessage"] = "Nie znaleziono użytkownika";
            return View();
        }

        public IActionResult WyswietlKonto(int? id)
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity != null &&
               claimUser.Identity.IsAuthenticated &&
               User.Claims.FirstOrDefault(c => c.Type == "Role").Value != "Klient")
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }

                var pracownik = _db.Pracownicy.Find(id);
                if (pracownik == null)
                {
                    return NotFound();
                }
                return View(pracownik);
            }
            return RedirectToAction("Index", "Home");
        }

        //GET
        public IActionResult ChangePassword(int? id)
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity == null ||
                !claimUser.Identity.IsAuthenticated ||
                User.Claims.FirstOrDefault(c => c.Type == "Role").Value == "Klient") return RedirectToAction("Index", "Home");

            var dane = _db.Pracownicy.Find(id);
            return View(dane);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(Pracownik obj, string pass, string pass1, string pass2)
        {
            if (ModelState.IsValid)
            {
                bool confirm = pass1.Equals(pass2);
                if (hashPassword(pass).Equals(obj.Haslo) && confirm
                    && hashPassword(pass1) != obj.Haslo && pass1.Length >= 6)
                {
                    _db.Database.ExecuteSqlRaw("UPDATE Pracownicy SET Haslo = '" + hashPassword(pass1) + "' WHERE Id = '" + obj.Id + "';");
                    TempData["success"] = "Zaktualizowano hasło";
                    return View("WyswietlKonto", obj);
                }
                else if (!hashPassword(pass).Equals(obj.Haslo)) TempData["error"] = "Błędne hasło";
                else if (!confirm) TempData["error"] = "Hasła nie są takie same";
                else if (hashPassword(pass1) == obj.Haslo) TempData["error"] = "Nowe hasło nie może być takie jak stare";
                else if (pass1.Length < 6) TempData["error"] = "Hasło jest za krótkie";

                return View(obj);
            }
            return View(obj);
        }
    }
}
