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

        public bool permission()
        {
            if (HttpContext.User.Identity != null &&
               HttpContext.User.Identity.IsAuthenticated &&
               User.Claims.FirstOrDefault(c => c.Type == "Role").Value == "Mechanik") return true;
            else return false;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
