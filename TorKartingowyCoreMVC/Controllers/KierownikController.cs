using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Drawing;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using TorKartingowyCoreMVC.Data;
using TorKartingowyCoreMVC.Models;

namespace TorKartingowyCoreMVC.Controllers
{
    public class KierownikController : Controller
    {
        private readonly ApplicationDbContext _db;

        public KierownikController(ApplicationDbContext db)
        {
            _db = db;
        }

        string hashPassword(string password)
        {
            var sha = SHA256.Create();
            var asByteArray = Encoding.Default.GetBytes(password);
            var hashedPassword = sha.ComputeHash(asByteArray);
            return Convert.ToBase64String(hashedPassword);
        }

        public bool permission()
        {
            if (HttpContext.User.Identity != null &&
               HttpContext.User.Identity.IsAuthenticated &&
               User.Claims.FirstOrDefault(c => c.Type == "Role").Value == "Kierownik") return true;
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

        public IActionResult ListaPracownikow()
        {
            if (permission())
            {
                IEnumerable<Pracownik> objPracownikList = _db.Pracownicy.Where(x => x.Stanowisko != "Kierownik").ToList();
                return View(objPracownikList);
            }
            else return RedirectToAction("Index", "Home");
            
        }

        //GET
        public IActionResult CreatePracownik()
        {
            if (permission())
            {
                var stanowiska = new List<string>()
                {
                    "Kasjer",
                    "Sprzętowiec",
                    "Instruktor",
                    "Mechanik",
                    "Kierownik"
                };
                ViewData["Stanowiska"] = stanowiska;
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePracownik(Pracownik obj)
        {
            if (ModelState.IsValid)
            {
                obj.Haslo = hashPassword(obj.Haslo);
                _db.Pracownicy.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Dodano pracownika";
                return RedirectToAction("ListaPracownikow");
            }
            return View(obj);
        }

        //GET
        public IActionResult EditPracownik(int? id)
        {
            if (permission())
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }
                var pracownikFromDb = _db.Pracownicy.Find(id);

                if (pracownikFromDb == null)
                {
                    return NotFound();
                }
                var stanowiska = new List<string>()
                {
                    "Kasjer",
                    "Sprzętowiec",
                    "Instruktor",
                    "Mechanik",
                    "Kierownik"
                };
                ViewData["Stanowiska"] = stanowiska;
                return View(pracownikFromDb);
            }
            else return RedirectToAction("Index", "Home");
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPracownik(Pracownik obj)
        {
            if (ModelState.IsValid)
            {
                _db.Pracownicy.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Zaktualizowano dane pracownika";
                return RedirectToAction("ListaPracownikow", "Kierownik");
            }
            return View(obj);
        }

        //GET
        public IActionResult DeletePracownik(int? id)
        {
            if (permission())
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }
                var pracownikFromDb = _db.Pracownicy.Find(id);

                if (pracownikFromDb == null)
                {
                    return NotFound();
                }

                return View(pracownikFromDb);
            }
            else return RedirectToAction("Index", "Home");
            
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePracownikPOST(Pracownik obj)
        {
            if (obj == null)
            {
                return NotFound();
            }
            _db.Database.ExecuteSqlRaw("UPDATE Rezerwacje SET PracownikId = " + User.Claims.FirstOrDefault(c => c.Type == "Numer").Value + " WHERE PracownikId = '" + obj.Id + "';");
            _db.Database.ExecuteSqlRaw("UPDATE Serwisy SET InstruktorId = " + User.Claims.FirstOrDefault(c => c.Type == "Numer").Value + " WHERE InstruktorId = '" + obj.Id + "';");
            _db.Database.ExecuteSqlRaw("UPDATE Serwisy SET MechanikId = " + User.Claims.FirstOrDefault(c => c.Type == "Numer").Value + " WHERE MechanikId = '" + obj.Id + "';");
            _db.Database.ExecuteSqlRaw("UPDATE RejestrPrac SET PracownikId = " + User.Claims.FirstOrDefault(c => c.Type == "Numer").Value + " WHERE PracownikId = '" + obj.Id + "';");
            _db.Pracownicy.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Usunięto pracownika";
            return RedirectToAction("ListaPracownikow");
        }

        //GOKART----------------------------------------------------------------------
        public IActionResult ListaGokartow()
        {
            if (permission())
            {
                IEnumerable<Gokart> objGokartList = _db.Gokarty;
                return View(objGokartList);
            }
            else return RedirectToAction("Index", "Home");
        }

        //GET
        public IActionResult CreateGokart()
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
        public IActionResult CreateGokart(Gokart obj)
        {
            if (ModelState.IsValid)
            {
                _db.Gokarty.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Dodano gokarta";
                return RedirectToAction("ListaGokartow");
            }
            return View(obj);
        }

        //GET
        public IActionResult EditGokart(int? id)
        {
            if (permission())
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }
                var gokartFromDb = _db.Gokarty.Find(id);

                if (gokartFromDb == null)
                {
                    return NotFound();
                }
                return View(gokartFromDb);
            }
            else return RedirectToAction("Index", "Home"); 
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditGokart(Gokart obj)
        {
            if (ModelState.IsValid)
            {
                _db.Gokarty.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Zaktualizowano dane gokarta";
                return RedirectToAction("ListaGokartow", "Kierownik");
            }
            return View(obj);
        }

        //GET
        public IActionResult DeleteGokart(int? id)
        {
            if (permission())
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }
                var gokartFromDb = _db.Gokarty.Find(id);

                if (gokartFromDb == null)
                {
                    return NotFound();
                }
                return View(gokartFromDb);
            }
            else return RedirectToAction("Index", "Home");
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteGokartPOST(Gokart obj)
        {
            if (obj == null)
            {
                return NotFound();
            }
            _db.Gokarty.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Usunięto gokarta";
            return RedirectToAction("ListaGokartow");
        }

        //------------------------------TOR---------------------------------
        public IActionResult ListaTorow()
        {
            if (permission())
            {
                IEnumerable<Tor> objTorList = _db.Tory;
                return View(objTorList);
            }
            else return RedirectToAction("Index", "Home");
        }

        //GET
        public IActionResult CreateTor()
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
        public IActionResult CreateTor(Tor obj)
        {
            if (ModelState.IsValid)
            {
                _db.Tory.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Dodano tor";
                return RedirectToAction("ListaTorow");
            }
            return View(obj);
        }

        //GET
        public IActionResult EditTor(int? id)
        {
            if (permission())
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }
                var TorFromDb = _db.Tory.Find(id);

                if (TorFromDb == null)
                {
                    return NotFound();
                }

                return View(TorFromDb);
            }
            else return RedirectToAction("Index", "Home");
            
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTor(Tor obj)
        {
            if (ModelState.IsValid)
            {
                _db.Tory.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Zaktualizowano dane toru";
                return RedirectToAction("ListaTorow", "Kierownik");
            }
            return View(obj);
        }

        //GET
        public IActionResult DeleteTor(int? id)
        {
            if (permission())
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }
                var torFromDb = _db.Tory.Find(id);

                if (torFromDb == null)
                {
                    return NotFound();
                }

                return View(torFromDb);
            }
            else return RedirectToAction("Index", "Home");
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteTorPOST(Tor obj)
        {
            if (obj == null)
            {
                return NotFound();
            }
            _db.Tory.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Usunięto tor";
            return RedirectToAction("ListaTorow");
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
                var query = from x in _db.RejestrPrac select x;
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
                var Pracownik = _db.Pracownicy.Where(x => x.Id == RejestrFromDb.PracownikId).First();
                ViewData["Pracownik"] = Pracownik.Imie + " " + Pracownik.Nazwisko;
                return View(RejestrFromDb);
            }
            else return RedirectToAction("Index", "Home");
        }

        //GET
        public IActionResult EditRejestr(int? id)
        {
            if (permission())
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }
                var WpisFromDb = _db.RejestrPrac.Find(id);

                if (WpisFromDb == null)
                {
                    return NotFound();
                }

                return View(WpisFromDb);
            }
            else return RedirectToAction("Index", "Home");

        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditRejestr(RejestrPrac obj)
        {
            if (ModelState.IsValid)
            {
                _db.RejestrPrac.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Zaktualizowano wpis";
                return RedirectToAction("ListaRejestr", "Kierownik");
            }
            return View(obj);
        }

        //----------------------Klient------------------------------

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

        //----------------------Harmonogram-------------------------

        public IActionResult ListaHarmonogram()
        {
            if (permission())
            {
                var culture = new CultureInfo("pl-PL");
                ViewData["Culture"] = culture;
                IEnumerable<Harmonogram> objHarmonogramList = _db.Harmonogram;
                return View(objHarmonogramList);
            }
            else return RedirectToAction("Index", "Home");
        }

        //GET
        public IActionResult CreateHarmonogram()
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
        public async Task<IActionResult> CreateHarmonogram(Harmonogram obj)
        {
            if (obj.OdKiedy != null)
            {
                var files = HttpContext.Request.Form.Files;
                if(files.Count() > 0)
                {
                    byte[] img = null;
                    using(var filestream = files[0].OpenReadStream())
                    {
                        using (var memorystream = new MemoryStream())
                        {
                            filestream.CopyTo(memorystream);
                            img = memorystream.ToArray();
                        }
                    }
                    obj.ListaGodzin = img;
                }
                _db.Harmonogram.Add(obj);
                await _db.SaveChangesAsync();
                TempData["success"] = "Dodano harmonogram";
                return RedirectToAction("ListaHarmonogram");
            }
            return View(obj);
        }

        public IActionResult HarmonogramDetails(string? data)
        {
            if (permission())
            {
                if (data == null) return NotFound();
                Harmonogram harmonogram = _db.Harmonogram.Find(data);
                return View(harmonogram);
            }
            return RedirectToAction("Index", "Home");
        }

        //GET
        public IActionResult DeleteHarmonogram(string? data)
        {
            if (permission())
            {
                if (data == null)
                {
                    return NotFound();
                }
                var harmonogram = _db.Harmonogram.Find(data);

                if (harmonogram == null)
                {
                    return NotFound();
                }

                return View(harmonogram);
            }
            else return RedirectToAction("Index", "Home");
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteHarmonogramPOST(Harmonogram obj)
        {
            if (obj == null)
            {
                return NotFound();
            }
            _db.Harmonogram.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Usunięto harmonogram";
            return RedirectToAction("ListaHarmonogram");
        }

        public IActionResult Cennik()
        {
            if (permission())
            {
                Cennik cennik = _db.Cennik.Find(1);
                return View(cennik);
            }
            else return RedirectToAction("Index", "Home");
        }
        //GET
        public IActionResult EditCennik(int? id)
        {
            if (permission())
            {
                if(id!=1)return NotFound();
                var cennik = _db.Cennik.Find(1);
                return View(cennik);
            } else return RedirectToAction("Index", "Home");
        }
        //POST  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCennik(Cennik cennik)
        {
            if(ModelState.IsValid)
            {
                _db.Cennik.Update(cennik);
                _db.SaveChanges();
                TempData["success"] = "Zaktualizowano cennik";
                return RedirectToAction("Cennik", "Kierownik");
            }
            return View(cennik);
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
    }

}
