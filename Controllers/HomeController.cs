using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using ClinicaVeterinaria.Models;

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
                    return RedirectToAction("Index", "Pharmacists");
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
            return RedirectToAction("Index");
        }
        //Lorenzo-Daniel
    }
}
