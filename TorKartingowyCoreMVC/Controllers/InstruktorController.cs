using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TorKartingowyCoreMVC.Data;
using TorKartingowyCoreMVC.Models;

namespace TorKartingowyCoreMVC.Controllers
{
    public class InstruktorController : Controller
    {
        private readonly ApplicationDbContext _db;

        public InstruktorController(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool permission()
        {
            if (HttpContext.User.Identity != null &&
               HttpContext.User.Identity.IsAuthenticated &&
               User.Claims.FirstOrDefault(c => c.Type == "Role").Value == "Instruktor") return true;
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
                if (!String.IsNullOrEmpty(searchFilter))
                {
                    query = query.Where(x => string.Concat(x.Imie, " ", x.Nazwisko).Contains(searchFilter) ||
                                        x.Numer.ToString().Contains(searchFilter) || x.Email.Contains(searchFilter) ||
                                        x.Telefon.Contains(searchFilter));
                }
                return View(await query.AsNoTracking().ToListAsync());
            }
            else return RedirectToAction("Index", "Home");
        }

        //GET
        public IActionResult UpdateSzkolenie(int? idKlienta)
        {
            if (permission())
            {
                if (idKlienta == null || idKlienta == 0)
                {
                    return NotFound();
                }
                var klientFromDb = _db.Klienci.Find(idKlienta);
                ViewData["DaneKlienta"] = "Klient: " + klientFromDb.Imie + " " + klientFromDb.Nazwisko;
                ViewData["NumerKlienta"] = "Numer klienta: " + klientFromDb.Numer;
                return View(klientFromDb);
            }
            else return RedirectToAction("Index", "Home");
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateSzkolenie(Klient obj)
        {
            if (ModelState.IsValid)
            {
                _db.Klienci.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Zaktualizowano szkolenie klienta nr " + obj.Numer;
                return RedirectToAction("ListaKlientow", "Instruktor");
            }
            return View(obj);
        }

        public IActionResult ListaSerwisow()
        {
            if (permission())
            {
                var instruktorId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == "Numer").Value);
                IEnumerable<Serwis> objSerwisList = _db.Serwisy.Where(r => r.InstruktorId == instruktorId).AsNoTracking().ToList(); ;
                return View(objSerwisList);
            }
            else return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ListaSerwisow(string searchFilter)
        {
            if (permission())
            {
                ViewData["GetSerwis"] = searchFilter;
                int instruktorId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == "Numer").Value);
                var query = from x in _db.Serwisy where x.InstruktorId == instruktorId select x;
                if (!String.IsNullOrEmpty(searchFilter))
                {
                    query = query.Where(x => x.DataUtworzenia.ToString().Contains(searchFilter) ||
                                        x.GokartNumer.ToString().Contains(searchFilter) || x.Usterka.Contains(searchFilter));
                }
                return View(await query.AsNoTracking().ToListAsync());
            }
            else return RedirectToAction("Index", "Home");
        }

        //GET
        public IActionResult CreateSerwis()
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
        public IActionResult CreateSerwis(Serwis obj)
        {
            if (ModelState.IsValid)
            {
                _db.Serwisy.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Dodano serwis dla gokarta " + obj.GokartNumer;
                return RedirectToAction("ListaSerwisow");
            }
            return View(obj);
        }

        //GET
        public IActionResult UpdateSerwis(int? id)
        {
            if (permission())
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }
                var serwisFromDb = _db.Serwisy.Find(id);
                
                if (serwisFromDb == null)
                {
                    return NotFound();
                }
                var temp = serwisFromDb.DataUtworzenia.ToString();
                ViewData["Data"] = temp.Substring(0, 10) ;
                return View(serwisFromDb);
            }
            else return RedirectToAction("Index", "Home");
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateSerwis(Serwis obj)
        {
            if (ModelState.IsValid)
            {
                _db.Serwisy.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Zaktualizowano serwis";
                return RedirectToAction("ListaSerwisow", "Instruktor");
            }
            return View(obj);
        }

        //----------------REJESTR PRAC------------------------------
        public IActionResult ListaRejestr()
        {
            if (permission())
            {
                var instruktorId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == "Numer").Value);
                IEnumerable<RejestrPrac> objRejestrList = _db.RejestrPrac.Where(r => r.PracownikId == instruktorId).AsNoTracking().ToList(); ;
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
    }
}
