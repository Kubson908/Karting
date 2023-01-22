using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using TorKartingowyCoreMVC.Data;
using TorKartingowyCoreMVC.Models;

namespace TorKartingowyCoreMVC.Controllers
{
    public class KlientController : Controller
    {
        private readonly ApplicationDbContext _db;
        public KlientController(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool permission()
        {
            if (HttpContext.User.Identity != null &&
               HttpContext.User.Identity.IsAuthenticated &&
               User.Claims.FirstOrDefault(c => c.Type == "Role").Value == "Klient") return true;
            else return false;
        }


        public IActionResult ListaRezerwacji()
        {
            if (permission())
            {
                var klientId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == "Numer").Value);
                IEnumerable<Rezerwacja> rezerwacje = _db.Rezerwacje.Where(r => r.KlientNumer == klientId).AsNoTracking().ToList();
                return View(rezerwacje);
            }
            else return RedirectToAction("Login", "Access");
        }

        //GET
        public IActionResult Rezerwuj1()
        {
            if (permission())
            {
                return View();
            }
            else return RedirectToAction("Login", "Access");
            
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Rezerwuj2(Rezerwacja obj)
        {
            if (ModelState.IsValid && DateOnly.Parse(obj.Data) > DateOnly.FromDateTime(DateTime.Now))
            {
                var lista = new List<string>();
                string key = "T" + obj.TorId.ToString() + "_" + obj.Data.Replace('-', '_');
                bool exists = _db.DostepneGodziny.Any(o => o.TorData == key);
                if (exists)
                {
                    DostepneGodziny termin = _db.DostepneGodziny.Find(key);
                    foreach (var col in termin.GetType().GetProperties().Where(p => p.Name.StartsWith("G")))
                    {
                        int hour = Int32.Parse(col.Name.Substring(1));
                        
                        bool available = true;
                        for (int i = hour; i < hour+obj.Czas; i++)
                        {
                            if (i > 20) break;
                            string name;
                            if (i < 10) name = "G0" + i.ToString();
                            else name = "G" + i.ToString();
                            var value = _db.Entry(termin).Property(name).CurrentValue.ToString();

                            if (Int32.Parse(value) + obj.LiczbaOsob <= 20) continue;
                            else
                            {
                                available = false;
                                break;
                            }
                        }
                        if (available && hour + obj.Czas <= 21)
                        {
                            lista.Add(col.Name);
                        }
                    }
                } else 
                {
                    DostepneGodziny temp = new DostepneGodziny();
                    foreach (var col in temp.GetType().GetProperties().Where(p => p.Name != "TorData"))
                    {
                        int hour = Int32.Parse(col.Name.Substring(1));

                        bool available = true;
                        for (int i = hour; i < hour + obj.Czas; i++)
                        {
                            if (i > 20) break;
                            if (obj.LiczbaOsob > 20)
                            {
                                available = false;
                                break;
                            }
                        }
                        if (available && hour + obj.Czas <= 21)
                        {
                            lista.Add(col.Name);
                        }
                    }
                }
                if (lista.Count == 0)
                {
                    TempData["error"] = "Termin niedostępny dla wskazanej liczby godzin i osób";
                    return RedirectToAction("Rezerwuj1");
                }
                ViewData["Godziny"] = lista;
                return View(obj);
            } else
            {
                TempData["error"] = "Wprowadź prawidłowe dane";
                return RedirectToAction("Rezerwuj1");
            }
            
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Rezerwuj3(Rezerwacja obj)
        {
            if (ModelState.IsValid)
            {
                Tor tor = _db.Tory.Find(obj.TorId);
                int spalinowe = tor.Pojemnosc;
                int elektryczne = tor.Pojemnosc;
                int dlaDzieci = tor.Pojemnosc;
                List<Gokart> gokarty = _db.Gokarty.Where(g => g.Typ == "Spalinowy").ToList();
                if (gokarty.Count <= tor.Pojemnosc) spalinowe = gokarty.Count - 2;
                gokarty = _db.Gokarty.Where(g => g.Typ == "Elektryczny").ToList();
                if (gokarty.Count <= tor.Pojemnosc) elektryczne = gokarty.Count - 2;
                gokarty = _db.Gokarty.Where(g => g.Typ == "Dla dzieci").ToList();
                if (gokarty.Count <= tor.Pojemnosc) dlaDzieci = gokarty.Count - 2;
                gokarty.Clear();
                string dataGodzina = obj.Data.Replace('-', '_') + "_" + obj.Godzina.Substring(1, 2);
                if (_db.IloscGokartow.Any(o => o.DataGodzina == dataGodzina))
                {
                    IloscGokartow iloscGokartow = _db.IloscGokartow.Find(dataGodzina);
                    spalinowe = spalinowe - iloscGokartow.Spalinowe;
                    elektryczne = elektryczne - iloscGokartow.Elektryczne;
                    dlaDzieci = dlaDzieci - iloscGokartow.DlaDzieci;
                }
                if (spalinowe > obj.LiczbaOsob) spalinowe = obj.LiczbaOsob;
                if (elektryczne > obj.LiczbaOsob) elektryczne = obj.LiczbaOsob;
                if (dlaDzieci > obj.LiczbaOsob) dlaDzieci = obj.LiczbaOsob;
                ViewData["Spalinowe"] = spalinowe;
                ViewData["Elektryczne"] = elektryczne;
                ViewData["DlaDzieci"] = dlaDzieci;
                if(spalinowe+elektryczne+dlaDzieci < obj.LiczbaOsob)
                {
                    TempData["error"] = "Brak dostępnych gokartów o godzinie " + obj.Godzina;
                    return Rezerwuj2(obj);
                }
                return View(obj);
            } else
            {
                TempData["error"] = "Wprowadź prawidłowe dane";
                return Rezerwuj2(obj);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Rezerwuj(Rezerwacja obj, int spalinowe, int elektryczne, int dla_dzieci, int s, int e, int d)
        {
            if (ModelState.IsValid)
            {
                if (spalinowe + elektryczne + dla_dzieci > obj.LiczbaOsob)
                {
                    TempData["error"] = "Ilość gokartów nie może być większa niż liczba osób";
                    ViewData["Spalinowe"] = s;
                    ViewData["Elektryczne"] = e;
                    ViewData["DlaDzieci"] = d;
                    return View("Rezerwuj3" ,obj);
                }
                obj.Godzina = obj.Godzina.Substring(1) + ":00";
                _db.Rezerwacje.Add(obj);
                _db.SaveChanges();
                string key = "T" + obj.TorId.ToString() + "_" + obj.Data.Replace('-', '_');
                string godzina = obj.Godzina.Substring(0, 2);
                if (!_db.DostepneGodziny.Any(o => o.TorData == key))
                {
                    _db.Database.ExecuteSqlRaw("INSERT INTO DostepneGodziny (TorData, G" + godzina + ") VALUES ('" + key + "', '" + obj.LiczbaOsob + "');");
                    int hour = Int32.Parse(godzina);
                    for (int i = hour + 1; i < hour + obj.Czas; i++)
                    {
                        string name;
                        if (i < 10) name = "G0" + i.ToString();
                        else name = "G" + i.ToString();
                        _db.Database.ExecuteSqlRaw("UPDATE DostepneGodziny SET " + name + " = " + name + " + " + obj.LiczbaOsob + " WHERE TorData = '" + key + "';");
                    }
                }
                else
                {
                    _db.Database.ExecuteSqlRaw("UPDATE DostepneGodziny SET G" + godzina + " = G" + godzina + " + " + obj.LiczbaOsob + " WHERE TorData = '" + key + "';");
                    int hour = Int32.Parse(godzina);
                    for (int i = hour + 1; i < hour + obj.Czas; i++)
                    {
                        string name;
                        if (i < 10) name = "G0" + i.ToString();
                        else name = "G" + i.ToString();
                        _db.Database.ExecuteSqlRaw("UPDATE DostepneGodziny SET " + name + " = " + name + " + " + obj.LiczbaOsob + " WHERE TorData = '" + key + "';");
                    }
                }

                string dataGodzina = obj.Data.Replace('-', '_') + "_" + obj.Godzina.Substring(0, 1);
                if (_db.IloscGokartow.Any(o => o.DataGodzina == dataGodzina))
                {
                    _db.Database.ExecuteSqlRaw("UPDATE IloscGokartow SET Spalinowe = Spalinowe + " + spalinowe +
                        ", Elektryczne = Elektryczne + " + elektryczne + ", DLaDzieci = DlaDzieci + " + dla_dzieci +
                        " WHERE DataGodzina= '" + dataGodzina + "';");
                }
                else
                {
                    _db.Database.ExecuteSqlRaw("INSERT INTO IloscGokartow (DataGodzina, Spalinowe, Elektryczne, DlaDzieci) " +
                        "VALUES ('" + dataGodzina + "', '" + spalinowe + "', '" + elektryczne + "', '" + dla_dzieci + "');");
                }
                _db.SaveChanges();
                TempData["success"] = "Pomyślnie zarezerwowano";
                return RedirectToAction("Index", "Home");
            }
            return Rezerwuj3(obj);
        }
    }
}
