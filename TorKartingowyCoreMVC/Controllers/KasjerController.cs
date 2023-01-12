using Microsoft.AspNetCore.Mvc;
using TorKartingowyCoreMVC.Data;

namespace TorKartingowyCoreMVC.Controllers
{
    public class KasjerController : Controller
    {
        private readonly ApplicationDbContext _db;

        public KasjerController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SearchKlient(string searchFilter)
        {
            ViewData["GetKlient"] = searchFilter;
        }

        public IActionResult UpdateRezerwacja(int? id)
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
    }
}
