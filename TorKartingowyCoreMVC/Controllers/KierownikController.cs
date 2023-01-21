﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
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
                IEnumerable<Pracownik> objPracownikList = _db.Pracownicy;
                return View(objPracownikList);
            }
            else return RedirectToAction("Index", "Home");
            
        }

        //GET
        public IActionResult CreatePracownik()
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

        //TOR----------------------------------------------------------------------
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
    }
}
