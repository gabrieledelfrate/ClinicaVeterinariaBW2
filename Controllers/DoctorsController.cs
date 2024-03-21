using ClinicaVeterinaria.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ClinicaVeterinaria.Controllers
{
 // inizio codice MG
    public class DoctorsController : Controller
    {
        private DBContext db = new DBContext();
        public ActionResult AddHospitalization()
        {
            var beasts = db.Beasts.ToList();
            var doctors = db.Doctors.ToList();
            ViewBag.BeastList = new SelectList(beasts, "BeastID", "Nome");
            ViewBag.DoctorList = new SelectList(doctors, "DoctorID", "Nome");

            return View();
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
            ViewBag.Message = "Ricovero creato con succeso!";
            return RedirectToAction("ActiveHospitalizations", "Doctors");
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