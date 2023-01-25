using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TorKartingowyCoreMVC.Data;
using TorKartingowyCoreMVC.Models;

namespace TorKartingowyCoreMVC.Controllers
{
    public class SprzetowiecController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SprzetowiecController(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool permission()
        {
            if (HttpContext.User.Identity != null &&
               HttpContext.User.Identity.IsAuthenticated &&
               User.Claims.FirstOrDefault(c => c.Type == "Role").Value == "Sprzętowiec") return true;
            else return false;
        }

        public IActionResult Index()
        {
            if (permission()) return View();
            return RedirectToAction("Index", "Home");
        }

        //------------------MAGAZYN---------------------------------
        public IActionResult Magazyn()
        {
            if (!permission()) return RedirectToAction("Index", "Home");

            IEnumerable<Magazyn> magazyn = _db.Magazyn;

            return View(magazyn);
        }

        [HttpGet]
        public async Task<IActionResult> Magazyn(string searchFilter)
        {
            if (permission())
            {
                ViewData["GetMagazyn"] = searchFilter;
                var query = from x in _db.Magazyn select x;
                if (!String.IsNullOrEmpty(searchFilter))
                {
                    query = query.Where(x => string.Concat(x.Kategoria, " ", x.Nazwa).Contains(searchFilter) ||
                                        x.Opis.Contains(searchFilter) || x.Nazwa.Contains(searchFilter) ||
                                        x.Kategoria.Contains(searchFilter));
                }
                return View(await query.AsNoTracking().ToListAsync());
            }
            else return RedirectToAction("Index", "Home");
        }

        //GET
        public IActionResult CreateMagazyn()
        {
            if (!permission()) return RedirectToAction("Index", "Home");

            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateMagazyn(Magazyn obj)
        {
            if (ModelState.IsValid)
            {
                _db.Magazyn.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Dodano pozycję do magazynu";
                return RedirectToAction("Magazyn");
            }
            return View(obj);
        }

        [HttpGet]
        public async Task<IActionResult> Wypozycz(int id, int ilosc)
        {
            if(!permission()) return RedirectToAction("Index", "Home");
            var element = _db.Magazyn.Find(id);
            if (element != null && element.StanMagazynowy - ilosc >= 0 && element.Wypozyczono + ilosc >= 0)
            {
                element.Wypozyczono += ilosc;
                element.StanMagazynowy -= ilosc;
                _db.Update(element);
                TempData["success"] = "Zapisano stan magazynu";
            }
            else TempData["error"] = "Zbyt mała ilość w magazynie";
            
            await _db.SaveChangesAsync();
            return RedirectToAction("Magazyn");
        }

        //GET
        public IActionResult EditMagazyn(int? id)
        {
            if (!permission()) return RedirectToAction("Index", "Home");

            if (id == null || id == 0)
            {
                return NotFound();
            }
            var element = _db.Magazyn.Find(id);
            if (element == null)
            {
                return NotFound();
            }
            return View(element);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditMagazyn(Magazyn obj)
        {
            if (ModelState.IsValid)
            {
                _db.Magazyn.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Zapisano pozycję nr " + obj.Id;
                return RedirectToAction("Magazyn");
            }
            return View(obj);
        }

        //GET
        public IActionResult DeleteMagazyn(int? id)
        {
            if (permission())
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }
                var magazyn = _db.Magazyn.Find(id);

                if (magazyn == null)
                {
                    return NotFound();
                }
                return View(magazyn);
            }
            else return RedirectToAction("Index", "Home");
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteMagazynPOST(Magazyn obj)
        {
            if (obj == null)
            {
                return NotFound();
            }
            _db.Magazyn.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Usunięto pozycję nr " + obj.Id;
            return RedirectToAction("Magazyn");
        }

        //----------------REJESTR PRAC------------------------------
        public IActionResult ListaRejestr()
        {
            if (!permission()) return RedirectToAction("Index", "Home");

            var pracownikId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == "Numer").Value);
            IEnumerable<RejestrPrac> objRejestrList = _db.RejestrPrac.Where(r => r.PracownikId == pracownikId).AsNoTracking().ToList(); ;
            return View(objRejestrList);
        }

        [HttpGet]
        public async Task<IActionResult> ListaRejestr(string searchFilter)
        {
            if (!permission()) return RedirectToAction("Index", "Home");

            ViewData["GetRejestr"] = searchFilter;
            int pracownikId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == "Numer").Value);
            var query = from x in _db.RejestrPrac where x.PracownikId == pracownikId select x;
            if (!String.IsNullOrEmpty(searchFilter))
            {
                query = query.Where(x => x.Data.ToString().Contains(searchFilter));
            }
            return View(await query.AsNoTracking().ToListAsync());
        }

        //GET
        public IActionResult CreateRejestr()
        {
            if (!permission()) return RedirectToAction("Index", "Home");

            return View();
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
            if (!permission()) return RedirectToAction("Index", "Home");

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

        public IActionResult Harmonogram()
        {
            if (!permission()) return RedirectToAction("Index", "Home");

            string data = null;
            foreach (Harmonogram h in _db.Harmonogram)
            {
                if (data == null) data = h.OdKiedy;
                else if (DateOnly.Parse(h.OdKiedy) > DateOnly.Parse(data)) data = h.OdKiedy;
            }
            Harmonogram harmonogram = _db.Harmonogram.Find(data);
            return View(harmonogram);
        }
    }
}
