using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using TorKartingowyCoreMVC.Data;
using TorKartingowyCoreMVC.Models;

namespace TorKartingowyCoreMVC.Controllers
{
    public class KlientController : Controller
    {
        private readonly ApplicationDbContext _db;
        public KlientController(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool permission()
        {
            if (HttpContext.User.Identity != null &&
               HttpContext.User.Identity.IsAuthenticated &&
               User.Claims.FirstOrDefault(c => c.Type == "Role").Value == "Klient") return true;
            else return false;
        }

        public IActionResult Index()
        {
            if (permission())
            {
                IEnumerable<Klient> objKlientList = _db.Klienci;
                return View(objKlientList);
            }
            else return RedirectToAction("Index", "Home");
        }

        public IActionResult ListaRezerwacji()
        {
            if (permission())
            {
                var klientId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == "Numer").Value);
                IEnumerable<Rezerwacja> rezerwacje = _db.Rezerwacje.Where(r => r.KlientNumer == klientId).AsNoTracking().ToList();
                return View(rezerwacje);
            }
            else return RedirectToAction("Login", "Access");
        }

        //GET
        public IActionResult Rezerwuj1()
        {
            if (permission())
            {
                return View();
            }
            else return RedirectToAction("Login", "Access");
            
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Rezerwuj2(Rezerwacja obj)
        {
            if (ModelState.IsValid)
            {
                ViewData["ListaGodzin"] = new List<string>();
                //if(_db.DostepneGodziny.Any(o => o.TorData == obj.Tor.ToString() + "_" + obj.Data))
                //{
                //    var termin = _db.DostepneGodziny.Find(obj.Tor.ToString() + "_" + obj.Data);
                //    foreach(var col in termin.GetType().GetProperties().Where(p => p.Name.StartsWith("G")))
                //    {
                //        int value = col.GetValue(this, null);
                //        if(value <= 20)
                //        {

                //        }
                //    }
                //}
                return View(obj);
            }
            return RedirectToAction("Rezerwuj1");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Rezerwuj(Rezerwacja obj)
        {
            if (ModelState.IsValid)
            {
                _db.Rezerwacje.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Pomyślnie zarezerwowano";
                return RedirectToAction("Index", "Home");
            }
            return Rezerwuj2(obj);
        }
    }
}
