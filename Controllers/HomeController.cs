using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using System.Web.Security;
using ClinicaVeterinaria.Models;
using System.Collections.Generic;


namespace ClinicaVeterinaria.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }

        //Lorenzo-Daniel
        public ActionResult LoginFarmacia()
        {
          
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult LoginDoctor()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

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
        
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public ActionResult LoginFarmacia(string email, string passwordFarmacista)
        {
          
            using (var db = new DBContext())
            {
                var pharmacist = db.Pharmacists.FirstOrDefault(p => p.Email == email && p.PasswordFarmacista == passwordFarmacista);

                if (pharmacist != null)
                {
                    FormsAuthentication.SetAuthCookie(email, false);

                    Session["PharmacistID"] = pharmacist.PharmacistID;
                    Session["UserEmail"] = email;
                    Session["UserF"] = pharmacist.Nome;
                    return RedirectToAction("Index", "Pharmacists");

                }
                else
                {
                    ViewBag.Error = "Credenziali non valide";
                    return View();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginDoctor(string email, string passwordDoctor)
        {
            using (var db = new DBContext())
            {
                var medico = db.Doctors.FirstOrDefault(m => m.Email == email && m.PasswordMedico == passwordDoctor);

                if (medico != null)
                {
                    FormsAuthentication.SetAuthCookie(email, false);

                    Session["DoctorID"] = medico.DoctorID;
                    Session["UserEmail"] = email;
                    Session["UserM"] = medico.Nome;
                    return RedirectToAction("Index", "Doctors");
                }
                else
                {
                    ViewBag.Error = "Credenziali non valide";
                    return View();
                }
            }

        }


        public ActionResult LogoutFarmacia()
        {
            FormsAuthentication.SignOut();

            if (Session["UserF"] != null)
            {
                Session.Remove("UserF");
            }

            if (Session["UserM"] != null)
            {
                Session.Remove("UserM");
            }

            return RedirectToAction("Index");
        }

        //Lorenzo-Daniel
    }
}
