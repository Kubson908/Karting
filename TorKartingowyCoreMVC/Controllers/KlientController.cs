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

        public IActionResult Index()
        {
            IEnumerable<Klient> objKlientList = _db.Klienci;
            return View(objKlientList);
        }

        public IActionResult ListaRezerwacji()
        {
            var klientId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == "Numer").Value);
            IEnumerable<Rezerwacja> rezerwacje = _db.Rezerwacje.Where(r => r.KlientNumer == klientId).AsNoTracking().ToList(); ;
            return View(rezerwacje);
        }

        //GET
        public IActionResult Rezerwuj1()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity != null &&
                claimUser.Identity.IsAuthenticated &&
                @User.Claims.FirstOrDefault(c => c.Type == "Role").Value == "Klient")
                return View();
            return RedirectToAction("Login", "Access");
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Rezerwuj2(Rezerwacja obj)
        {
            if (ModelState.IsValid)
            {
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

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Klient obj)
        {
            if (ModelState.IsValid)
            {
                _db.Klienci.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Dodano klienta";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var klientFromDb = _db.Klienci.Find(id);
            
            if (klientFromDb == null)
            {
                return NotFound();
            }

            return View(klientFromDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Klient obj)
        {
            if (ModelState.IsValid)
            {
                _db.Klienci.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Zaktualizowano dane klienta";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var klientFromDb = _db.Klienci.Find(id);

            if (klientFromDb == null)
            {
                return NotFound();
            }

            return View(klientFromDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(Klient obj)
        {
            if(obj == null)
            {
                return NotFound();
            }
            _db.Klienci.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Usunięto klienta";
            return RedirectToAction("Index");
        }
    }
}
