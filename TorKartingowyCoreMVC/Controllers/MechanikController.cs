using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TorKartingowyCoreMVC.Data;
using TorKartingowyCoreMVC.Models;

namespace TorKartingowyCoreMVC.Controllers
{
    public class MechanikController : Controller
    {
        private readonly ApplicationDbContext _db;

        public MechanikController(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool permission()
        {
            if (HttpContext.User.Identity != null &&
               HttpContext.User.Identity.IsAuthenticated &&
               User.Claims.FirstOrDefault(c => c.Type == "Role").Value == "Mechanik") return true;
            else return false;
        }

        public IActionResult Index()
        {
            if (permission())
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

		//----------------SERWIS------------------------------------
		public IActionResult ListaSerwisow()
		{
			if (permission())
			{
				IEnumerable<Serwis> objSerwisList = _db.Serwisy;
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
				var query = from x in _db.Serwisy select x;
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
                List<Gokart> gokarty = _db.Gokarty.ToList();
                List<int> numery = new List<int>();
                foreach (Gokart g in gokarty)
                {
                    numery.Add(g.Numer);
                }
                ViewData["Numery"] = numery;
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
                List<Gokart> gokarty = _db.Gokarty.ToList();
                List<int> numery = new List<int>();
                foreach (Gokart g in gokarty)
                {
                    numery.Add(g.Numer);
                }
                ViewData["Numery"] = numery;
                var temp = serwisFromDb.DataUtworzenia.ToString();
				ViewData["Data"] = temp.Substring(0, 10);
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
				return RedirectToAction("ListaSerwisow", "Mechanik");
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
    }
}
