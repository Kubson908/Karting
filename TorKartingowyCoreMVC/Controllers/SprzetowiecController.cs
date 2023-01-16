using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
               User.Claims.FirstOrDefault(c => c.Type == "Role").Value == "Sprzetowiec") return true;
            else return false;
        }

        public IActionResult Index()
        {
            return View();
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
    }
}
