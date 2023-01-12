using Microsoft.AspNetCore.Mvc;
using TorKartingowyCoreMVC.Data;

namespace TorKartingowyCoreMVC.Controllers
{
    public class MechanikController : Controller
    {
        private readonly ApplicationDbContext _db;

        public MechanikController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
