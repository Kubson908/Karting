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
                IEnumerable<Rezerwacja> rezerwacje = _db.Rezerwacje.Where(x => x.KlientNumer == idKlienta);
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
                TempData["success"] = "Zapisano rezerwację nr " + obj.Numer;
                var klientFromDb = _db.Klienci.Find(obj.KlientNumer);
                TempData["DaneKlienta"] = "Klient: " + klientFromDb.Imie + " " + klientFromDb.Nazwisko;
                TempData["NumerKlienta"] = "Numer klienta: " + klientFromDb.Numer;
                return RedirectToAction("ListaRezerwacji", "Kasjer", obj.KlientNumer);
            }
            return View(obj);
        }

        //----------------REJESTR PRAC------------------------------
        public IActionResult ListaRejestr()
        {
            if (permission())
            {
                var pracownikId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == "Numer").Value);
                IEnumerable<RejestrPrac> objRejestrList = _db.RejestrPrac.Where(r => r.PracownikId == pracownikId).AsNoTracking().ToList(); ;
                return View(objRejestrList);
            }
            else return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ListaRejestr(string searchFilter)
        {
            if (permission())
            {
                ViewData["GetRejestr"] = searchFilter;
                int pracownikId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == "Numer").Value);
                var query = from x in _db.RejestrPrac where x.PracownikId == pracownikId select x;
                if (!String.IsNullOrEmpty(searchFilter))
                {
                    query = query.Where(x => x.Data.ToString().Contains(searchFilter));
                }
                return View(await query.AsNoTracking().ToListAsync());
            }
            else return RedirectToAction("Index", "Home");
        }

        //GET
        public IActionResult CreateRejestr()
        {
            if (permission())
            {
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateRejestr(RejestrPrac obj)
        {
            if (ModelState.IsValid)
            {
                _db.RejestrPrac.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Dodano wpis w rejestrze";
                return RedirectToAction("ListaRejestr");
            }
            return View(obj);
        }

        //GET
        public IActionResult RejestrDetails(int? id)
        {
            if (permission())
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }
                var RejestrFromDb = _db.RejestrPrac.Find(id);

                if (RejestrFromDb == null)
                {
                    return NotFound();
                }

                return View(RejestrFromDb);
            }
            else return RedirectToAction("Index", "Home");
        }

        public IActionResult Harmonogram()
        {
            if (permission())
            {
                string data = null;
                foreach (Harmonogram h in _db.Harmonogram)
                {
                    if (data == null) data = h.OdKiedy;
                    else if (DateOnly.Parse(h.OdKiedy) > DateOnly.Parse(data)) data = h.OdKiedy;
                }
                Harmonogram harmonogram = _db.Harmonogram.Find(data);
                return View(harmonogram);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
