using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClinicaVeterinaria.Models;
using System.Data.Entity;
using System.Net;

namespace ClinicaVeterinaria.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult HospitalizationsSearch()
        {
            return View();
        }

        public ActionResult LoginPharmacist()
        {
            return View();
        }

        private DBContext db = new DBContext();

        [HttpGet]
        public ActionResult SearchByMicrochip(string MicrochipCodice)
        {
            try
            {
                int? beastID = db.Beasts
                                  .Where(b => b.MicrochipCodice == MicrochipCodice)
                                  .Select(b => (int?)b.BeastID)
                                  .FirstOrDefault();

                if (beastID != null)
                {
                    var hospitalizations = db.Hospitalizations
                                                .Where(h => h.BeastID == beastID)
                                                .ToList();
                    return PartialView("~/Views/Shared/Partials/_HospitalizationResults.cshtml", hospitalizations);
                }
                else
                {
                    return PartialView("~/Views/Shared/Partials/_HospitalizationResults.cshtml", new List<Hospitalization>());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
    }
}
