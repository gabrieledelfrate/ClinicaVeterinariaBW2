using ClinicaVeterinaria.Models;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;
using System;

namespace ClinicaVeterinaria.Controllers
{
    public class PharmacistsController : Controller
    {
        private DBContext db = new DBContext();

        
        public ActionResult Index(string search)
        {
           

            IQueryable<Product> products = db.Products;

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(s => s.Nome.Contains(search));
            }

            return View(products.ToList());
        }


        public ActionResult Details(int id)
        {
            var product = db.Products
                            .Include(p => p.Drawer)
                            .Include(p => p.Drawer.Locker)
                            .FirstOrDefault(p => p.ProductID == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult AddToReport(int id)
        {
            var product = db.Products.Find(id);
            if(product == null)
            {
                return HttpNotFound();
            }

            List<int> reportProducts = Session["ReportProducts"] as List<int>;
            if (reportProducts == null)
            {
                reportProducts = new List<int>();
            }

            reportProducts.Add(id);
            Session["ReportProducts"] = reportProducts;

            return RedirectToAction("Index");
        }

        public ActionResult Orders()
        {
            List<int> reportProductID = Session["ReportProducts"] as List<int>;
            List<Product> reportProducts = new List<Product>();

            if (reportProductID != null)
            {
                foreach (int productId in reportProductID)
                {
                    var product = db.Products.Find(productId);
                    if (product != null)
                    {
                        reportProducts.Add(product);
                    }
                }
            }

            var pharmacists = db.Pharmacists.Select(p => new
            {
                PharmacistID = p.PharmacistID,
                Name = p.Nome
            }).ToList();

            ViewBag.Pharmacists = new SelectList(pharmacists, "PharmacistID", "Name");

            return View(reportProducts);
        }


        public ActionResult RemoveFromReport(int id)
        {
            List<int> reportProducts = Session["ReportProducts"] as List<int>;
            if (reportProducts != null)
            {
                reportProducts.Remove(id);
                Session["ReportProducts"] = reportProducts;
            }

            return RedirectToAction("Orders");
        }

        [HttpPost]
        public ActionResult Checkout(string codiceFiscale, string numeroRicetta, int pharmacistId)
        {
            List<int> reportProductIDs = Session["ReportProducts"] as List<int>;
            if (reportProductIDs != null && reportProductIDs.Count > 0)
            {
                foreach (int productId in reportProductIDs)
                {
                    var product = db.Products.Find(productId);
                    if (product != null)
                    {
                        var sale = new Sale
                        {
                            ProductID = productId,
                            CodiceFiscale = codiceFiscale,
                            NumeroRicetta = numeroRicetta,
                            Prezzo = product.Prezzo,
                            PharmacistID = pharmacistId
                        };
                        db.Sales.Add(sale);
                    }
                }
                db.SaveChanges();

                var pharmacist = db.Pharmacists.FirstOrDefault(p => p.PharmacistID == pharmacistId);
                string nomeFarmacista = pharmacist != null ? pharmacist.Nome : "Nome non trovato";


                var summaryViewModel = new CheckoutSummaryViewModel
                {
                    NumeroRicetta = numeroRicetta,
                    NomeFarmacista = nomeFarmacista
                };

                Session["ReportProducts"] = null;

                TempData["SuccessMessage"] = "Checkout completato con successo.";
                return RedirectToAction("CheckoutSummary", new { codiceFiscale = codiceFiscale, numeroRicetta = numeroRicetta, pharmacistId = pharmacistId });
            }
            else
            {
                TempData["ErrorMessage"] = "Nessun prodotto nel resoconto.";
                return RedirectToAction("Orders");
            }
        }


        public ActionResult CheckoutSummary(string codiceFiscale, string numeroRicetta, int pharmacistId)
        {
            var summaryViewModel = new CheckoutSummaryViewModel();

            summaryViewModel.Animale = db.Beasts.FirstOrDefault(b => b.CodiceFiscale == codiceFiscale);

            summaryViewModel.Vendite = db.Sales
                                        .Where(s => s.CodiceFiscale == codiceFiscale)
                                        .ToList();

            summaryViewModel.PrezzoTotale = summaryViewModel.Vendite.Sum(v => v.Prezzo);
            summaryViewModel.NumeroRicetta = numeroRicetta;
            summaryViewModel.PharmacistID= pharmacistId;
            var pharmacist = db.Pharmacists.FirstOrDefault(p => p.PharmacistID == pharmacistId);
            summaryViewModel.NomeFarmacista = pharmacist != null ? pharmacist.Nome : "Nome non trovato";

            return View(summaryViewModel);
        }
    }
}
