
using System;
using System.IO;
using ClinicaVeterinaria.Models;
using Microsoft.Ajax.Utilities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace ClinicaVeterinaria.Controllers
{
    // inizio codice MG
    public class DoctorsController : Controller
    {
        private DBContext db = new DBContext();


        //Inizio Codice Pes
        public ActionResult Index(string search)
        {
            var beasts = db.Beasts.ToList();

            if (!string.IsNullOrEmpty(search))
            {
                beasts = beasts.Where(b => b.Nome.Contains(search) || b.MicrochipCodice == search).ToList();
            }

            ViewBag.SearchString = search;
            return View(beasts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddBeast(Beast beast, HttpPostedFileBase Foto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Foto != null && Foto.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(Foto.FileName);
                        string path = Path.Combine(Server.MapPath("~/assets/img/"), fileName);
                        Foto.SaveAs(path);
                        beast.Foto = "/assets/img/" + fileName;
                    }

                    db.Beasts.Add(beast);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Doctors");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Impossibile aggiungere la bestia. Si è verificato un errore: " + ex.Message);
                }
            }

            return View(beast);
        }

        //Fine Codice Pes
        [HttpGet]
        public ActionResult AddHospitalization()

        private DBContext db = new DBContext();
        // GET: Doctors
        // CODICE MARCO SILVERI

        [HttpGet]
        public ActionResult AddExamination()
        {
            var Bestie = db.Beasts.ToList();
            var Dottori = db.Doctors.ToList();
            ViewBag.Bestie = Bestie;
            ViewBag.Dottori = Dottori;
            return View();
        }
        [HttpGet]
        public ActionResult AddHospitalization()
        {
            var beasts = db.Beasts.ToList();
            var doctors = db.Doctors.ToList();
            ViewBag.BeastList = new SelectList(beasts, "BeastID", "Nome");
            ViewBag.DoctorList = new SelectList(doctors, "DoctorID", "Nome");

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

        [HttpPost]
        public ActionResult AddHospitalization(Hospitalization hospitalization)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Hospitalizations.Add(hospitalization);
                    db.SaveChanges();
                    ViewBag.Message = "Ricovero creato con succcesso!";
                    return RedirectToAction("ActiveHospitalizations", "Doctors");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Si é verificato un errore durante la creazione del ricovero ");
            }
            var userID = Convert.ToInt32(User.Identity.Name);
            hospitalization.BeastID = userID;

            db.Hospitalizations.Add(hospitalization);
            db.SaveChanges();
            ViewBag.Message = "Ricovero creato con successo!";
            return RedirectToAction("ActiveHospitalizations", "Doctors");
        }

        // FINE CODICE MARCO SILVERI
    }
}