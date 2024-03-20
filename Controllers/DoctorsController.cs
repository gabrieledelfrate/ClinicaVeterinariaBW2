using ClinicaVeterinaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicaVeterinaria.Controllers
{
    public class DoctorsController : Controller
    {
        private DBContext db = new DBContext();



        // Inizio Codice Pes

        public ActionResult Index()
        {
            var beasts = db.Beasts.ToList();
            return View(beasts);
        }

        public ActionResult AddBeast()
        {
            return View();
        }

        // Inizio Codice Pes

        public ActionResult AddBeast()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddBeast([Bind(Include = "DataRegistrazione,Nome,Tipologia,ColoreMantello,Foto,DataNascita,Presunta,Microchip,Smarrito,Proprietario,CodiceFiscale,EmailProprietario,CellulareProprietario,PatologiePregresse,MicrochipCodice")] Beast beast)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Beasts.Add(beast);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Beast");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Impossibile aggiungere la bestia. Si è verificato un errore: " + ex.Message);
                }
            }

            return View(beast);
        }


        // Fine Codice Pes
    }
}