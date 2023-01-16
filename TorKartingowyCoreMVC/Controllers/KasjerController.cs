using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TorKartingowyCoreMVC.Data;
using TorKartingowyCoreMVC.Models;

namespace TorKartingowyCoreMVC.Controllers
{
    public class KasjerController : Controller
    {
        private readonly ApplicationDbContext _db;

        public KasjerController(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool permission()
        {
            if (HttpContext.User.Identity != null &&
               HttpContext.User.Identity.IsAuthenticated &&
               User.Claims.FirstOrDefault(c => c.Type == "Role").Value == "Kasjer") return true;
            else return false;
        }

        public IActionResult Index()
        {
            if (permission())
            {
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }

        public IActionResult ListaKlientow()
        {
            if (permission())
            {
                IEnumerable<Klient> objKlientList = _db.Klienci;
                return View(objKlientList);
            }
            else return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ListaKlientow(string searchFilter)
        {
            if (permission())
            {
                ViewData["GetKlient"] = searchFilter;
                var query = from x in _db.Klienci select x;
                if(!String.IsNullOrEmpty(searchFilter))
                {
                    query = query.Where(x => string.Concat(x.Imie, " ", x.Nazwisko).Contains(searchFilter) || 
                                        x.Numer.ToString().Contains(searchFilter) || x.Email.Contains(searchFilter) ||
                                        x.Telefon.Contains(searchFilter));
                }
                return View(await query.AsNoTracking().ToListAsync());
            }
            else return RedirectToAction("Index", "Home");
        }

        public IActionResult ListaRezerwacji(int? idKlienta)
        {
            if (permission())
            {
                if (idKlienta == null || idKlienta == 0)
                {
                    IEnumerable<Rezerwacja> rezerwacjeFromDb = _db.Rezerwacje;
                    return View(rezerwacjeFromDb);
                }
                var klientFromDb = _db.Klienci.Find(idKlienta);
                ViewData["DaneKlienta"] = "Klient: " + klientFromDb.Imie + " " + klientFromDb.Nazwisko; 
                ViewData["NumerKlienta"]= "Numer klienta: " + klientFromDb.Numer;
                IEnumerable<Rezerwacja> rezerwacje = _db.Rezerwacje.Where(x => x.KlientNumer == idKlienta);;
                return View(rezerwacje);
            }
            else return RedirectToAction("Index", "Home");
        }

        //GET
        public IActionResult UpdateRezerwacja(int? id)
        {
            if (permission())
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }
                var rezerwacjaFromDb = _db.Rezerwacje.Find(id);

                if (rezerwacjaFromDb == null)
                {
                    return NotFound();
                }
                return View(rezerwacjaFromDb);
            }
            else return RedirectToAction("Index", "Home");
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateRezerwacja(Rezerwacja obj)
        {
            if (ModelState.IsValid)
            {
                _db.Rezerwacje.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Zrealizowano rezerwację nr " + obj.Numer;
                return RedirectToAction("ListaKlientow", "Kasjer");
            }
            return View(obj);
        }
    }
}
