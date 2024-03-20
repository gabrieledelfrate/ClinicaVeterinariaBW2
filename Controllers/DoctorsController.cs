using ClinicaVeterinaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace ClinicaVeterinaria.Controllers
{
    public class DoctorsController : Controller
    {

        private DBContext db = new DBContext();
        // GET: Doctors
        // CODICE MARCO SILVERI
        public ActionResult AddExamination()
        {
            var Bestie = db.Beasts.ToList();
            var Dottori = db.Doctors.ToList();
            ViewBag.Bestie = Bestie;
            ViewBag.Dottori = Dottori;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddExamination([Bind(Include = "ExaminationID, DataVisita, PatologieRiscontrate, NumeroRicetta, Ricetta, Prezzo, DoctorID, BeastID")]  Examination esame)
        {
            ViewBag.Dottori = db.Doctors.ToList();
            ViewBag.Bestie = db.Beasts.ToList();

            if (esame.DoctorID <= 0)
            {
                ModelState.AddModelError("DoctorID", "La selezione del dottore è obbligatoria.");
            }
            if (esame.BeastID <= 0)
            {
                ModelState.AddModelError("BeastID", "La selezione dell'animale è obbligatoria.");
            }

            if (ModelState.IsValid)
            {
                db.Examinations.Add(esame);
                db.SaveChanges();
                TempData["message"] = "Esame prenotato con successo";
                return RedirectToAction("Details", new { id = esame.BeastID });
            }
            else
            {
                TempData["messageError"] = "Errore nella compilazione dell'esame";
            }

            return View(esame);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var beast = db.Beasts
                           .Include(b => b.Examinations) 
                           .Include(b => b.Hospitalizations) 
                           .FirstOrDefault(b => b.BeastID == id);

            if (beast == null)
            {
                return HttpNotFound();
            }

            return View(beast);
        }

        // FINE CODICE MARCO SILVERI
    }
}