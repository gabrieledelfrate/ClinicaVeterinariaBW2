
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
using System.Xml.Linq;
using System.Drawing;

namespace ClinicaVeterinaria.Controllers
{
    // inizio codice MG
    public class DoctorsController : Controller
    {
        private DBContext db = new DBContext();


        [HttpGet]
        public ActionResult Index(string search)
        {
            var beasts = db.Beasts.ToList();

            if (!string.IsNullOrEmpty(search))
            {
                beasts = beasts.Where(b => b.Nome.Contains(search) || b.MicrochipCodice == search).ToList();
            }

            ViewBag.SearchString = search;
            ViewBag.NoResults = beasts.Count == 0;
            return View(beasts);
        }
        [HttpGet]
        public ActionResult AddBeast()
        {
            return View();
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

        public ActionResult DottoreDet(int? id)
        {
            if ( id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var dottore = db.Doctors.FirstOrDefault(d => d.DoctorID == id);
            if ( dottore == null)
            {
                return HttpNotFound();
            }

            return View(dottore);
        }

        [HttpPost]
        public ActionResult AddHospitalization(Hospitalization hospitalization)
        {
            try
            {
                ViewBag.DoctorList = new SelectList(db.Doctors.ToList(), "DoctorID", "Nome");
                ViewBag.BeastList = new SelectList(db.Beasts.ToList(), "BeastID", "Nome");
   
                if (hospitalization.DoctorID <= 0)
                {
                    ModelState.AddModelError("DoctorID", "La selezione del dottore è obbligatoria.");
                }
                if (hospitalization.BeastID <= 0)
                {
                    ModelState.AddModelError("BeastID", "La selezione della Bestia è obbligatoria.");
                }
                if (ModelState.IsValid)
                {
                    db.Hospitalizations.Add(hospitalization);
                    db.SaveChanges();
                    TempData["message"] = "Ricovero creato con succcesso!";
                    return RedirectToAction("Details", new { id = hospitalization.BeastID });
                }
                else
                {
                    TempData["messageError"] = "Errore nella compilazione del ricovero!";
                }
                return View(hospitalization);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Si è verificato un errore durante la creazione del ricovero: {ex.Message}");
            }
            return View(hospitalization);
        }

        public ActionResult ActiveHospitalizations()
        {
            DateTime currentDate = DateTime.Now;

            var allHospitalizations = db.Hospitalizations.ToList();
            return View(allHospitalizations);
        }
        public ActionResult GetHospitalizationDetails(int id)
        {
            var hospitalization = db.Hospitalizations.Find(id);
            if (hospitalization != null)
            {
                var details = new
                {
                    animalName = hospitalization.Beast.Nome,
                    doctorName = hospitalization.Doctor.Nome + " " + hospitalization.Doctor.Cognome,
                    prognosis = hospitalization.Prognosi,
                    dailyCost = hospitalization.CostoGiornalieroRicovero
                };
                return Json(details, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult CheckoutHospitalization(int id)
        {
            var hospitalization = db.Hospitalizations.Find(id);
            if (hospitalization != null)
            {
                hospitalization.DataFineRicovero = DateTime.Now.Date;
                db.SaveChanges();
                return Json(new { success = true });
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult> ContabilizzazioneRicoveriAsincrona()
        {
            try
            {
                var activeHospitalizations = await db.Hospitalizations
                    .Where(h => h.DataFineRicovero == null && h.Beast.Microchip)
                    .Select(h => new
                    {
                        AnimalName = h.Beast.Nome,
                        StartDate = h.DataInizioRicovero
                    })
                    .ToListAsync();

                return Json(activeHospitalizations, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        // Fine codice MG
    }
}