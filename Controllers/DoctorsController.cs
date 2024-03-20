using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClinicaVeterinaria.Models;

namespace ClinicaVeterinaria.Controllers
{
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
    }
}