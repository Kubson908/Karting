using Microsoft.AspNetCore.Mvc;
using TorKartingowyCoreMVC.Data;

namespace TorKartingowyCoreMVC.Controllers
{
    public class SprzetowiecController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SprzetowiecController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
