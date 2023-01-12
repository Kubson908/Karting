using Microsoft.AspNetCore.Mvc;
using TorKartingowyCoreMVC.Data;

namespace TorKartingowyCoreMVC.Controllers
{
    public class InstruktorController : Controller
    {
        private readonly ApplicationDbContext _db;

        public InstruktorController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
