using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicaVeterinaria.Controllers
{
    public class PharmacistsController : Controller
    {
        // GET: Pharmacists
        public ActionResult Index()
        {
            return View();
        }
    }
}