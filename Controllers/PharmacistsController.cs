using ClinicaVeterinaria.Models;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;
using System;
using System.Net;

namespace ClinicaVeterinaria.Controllers
{
    public class PharmacistsController : Controller
    {
        private DBContext db = new DBContext();


        public ActionResult Index(string search, string descriptionSearch)
        {
            var redirect = RedirectToLoginIfNotAuthenticated();
            if (redirect != null) return redirect;
            IQueryable<Product> products = db.Products;

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Nome.Contains(search));
                return View(products.ToList());
            }

            if (!string.IsNullOrEmpty(descriptionSearch))
            {
                products = products.Where(p => p.Descrizione.Contains(descriptionSearch));
                return View(products.ToList());
            }

            return View(products.ToList());
        }


        public ActionResult Details(int id)
        {
            var redirect = RedirectToLoginIfNotAuthenticated();
            if (redirect != null) return redirect;
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
            var redirect = RedirectToLoginIfNotAuthenticated();
            if (redirect != null) return redirect;
            var product = db.Products.Find(id);
            if (product == null)
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
            var redirect = RedirectToLoginIfNotAuthenticated();
            if (redirect != null) return redirect;
            List<int> reportProductID = Session["ReportProducts"] as List<int>;
            List<Product> reportProducts = new List<Product>();
            decimal totalPrice = 0;

            if (reportProductID != null)
            {
                foreach (int productId in reportProductID)
                {
                    var product = db.Products.Find(productId);
                    if (product != null)
                    {
                        reportProducts.Add(product);
                        totalPrice += product.Prezzo;
                    }
                }
            }

            ViewBag.TotalPrice = totalPrice;

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
            var redirect = RedirectToLoginIfNotAuthenticated();
            if (redirect != null) return redirect;
            List<int> reportProducts = Session["ReportProducts"] as List<int>;
            if (reportProducts != null)
            {
                reportProducts.Remove(id);
                Session["ReportProducts"] = reportProducts;
            }

            return RedirectToAction("Orders");
        }

        [HttpPost]
        public ActionResult Checkout(string codiceFiscale, string numeroRicetta, int pharmacistId, DateTime dataVendita)
        {
            var redirect = RedirectToLoginIfNotAuthenticated();
            if (redirect != null) return redirect;
            if (codiceFiscale.Length != 16)
            {
                TempData["ErrorMessage"] = "Il codice fiscale deve essere lungo 16 caratteri.";
                return RedirectToAction("Orders");
            }

            var animal = db.Beasts.FirstOrDefault(b => b.CodiceFiscale == codiceFiscale);
            if (animal == null)
            {
                TempData["ErrorMessage"] = "Il codice fiscale inserito non è associato a nessun animale.";
                return RedirectToAction("Orders");
            }

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
                            DataVendita = dataVendita,
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
                    NomeFarmacista = nomeFarmacista,
                    DataVendita = dataVendita
                };

                Session["ReportProducts"] = null;

                TempData["SuccessMessage"] = "Checkout completato con successo.";
                return RedirectToAction("CheckoutSummary", new { codiceFiscale = codiceFiscale, numeroRicetta = numeroRicetta, pharmacistId = pharmacistId, dataVendita = dataVendita });
            }
            else
            {
                TempData["ErrorMessage"] = "Nessun prodotto nel resoconto.";
                return RedirectToAction("Orders");
            }
        }


        public ActionResult CheckoutSummary(string codiceFiscale, string numeroRicetta, int pharmacistId, DateTime dataVendita)
        {
            var redirect = RedirectToLoginIfNotAuthenticated();
            if (redirect != null) return redirect;
            var summaryViewModel = new CheckoutSummaryViewModel();

            summaryViewModel.Animale = db.Beasts.FirstOrDefault(b => b.CodiceFiscale == codiceFiscale);

            var oggi = DateTime.Today;

            summaryViewModel.Vendite = db.Sales
                            .Where(s => s.CodiceFiscale == codiceFiscale &&
                                        s.DataVendita.Year == oggi.Year &&
                                        s.DataVendita.Month == oggi.Month &&
                                        s.DataVendita.Day == oggi.Day)
                            .OrderByDescending(s => s.DataVendita)
                            .ToList();

            summaryViewModel.PrezzoTotale = summaryViewModel.Vendite.Sum(v => v.Prezzo);
            summaryViewModel.DataVendita = dataVendita;
            summaryViewModel.NumeroRicetta = numeroRicetta;
            summaryViewModel.PharmacistID = pharmacistId;
            var pharmacist = db.Pharmacists.FirstOrDefault(p => p.PharmacistID == pharmacistId);
            summaryViewModel.NomeFarmacista = pharmacist != null ? pharmacist.Nome : "Nome non trovato";

            return View(summaryViewModel);
        }


        public ActionResult AddNewProduct()
        {
            var redirect = RedirectToLoginIfNotAuthenticated();
            if (redirect != null) return redirect;
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "NomeAzienda");
            ViewBag.DrawerID = new SelectList(db.Drawers, "DrawerID", "DrawerID");
            return View();
        }

        public ActionResult MedicinaXCliente()
        {

            var redirect = RedirectToLoginIfNotAuthenticated();
            if (redirect != null) return redirect;

            var proprietari = db.Beasts.Select(b => new
            {
                NomeProprietario = b.Proprietario,
                CodiceFiscale = b.CodiceFiscale
            }).Distinct().ToList();

            ViewBag.Proprietari = new SelectList(proprietari, "CodiceFiscale", "NomeProprietario");

            return View();
        }
        public ActionResult GetProdottiByCodiceFiscale(string codiceFiscale)
        {
            var redirect = RedirectToLoginIfNotAuthenticated();
            if (redirect != null) return redirect;
            var vendite = db.Sales
                            .Where(s => s.CodiceFiscale == codiceFiscale)
                            .Select(s => s.ProductID)
                            .Distinct()
                            .ToList();

            if (!vendite.Any())
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Nessuna vendita registrata per il codice fiscale inserito.");
            }

            var prodotti = db.Products
                             .Where(p => vendite.Contains(p.ProductID))
                             .ToList();

            return PartialView("_ProdottiByCodiceFiscale", prodotti);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewProduct([Bind(Include = "ProductID,Nome,Descrizione,SupplierID,DrawerID,Prezzo")] Product product)
        {
            var redirect = RedirectToLoginIfNotAuthenticated();
            if (redirect != null) return redirect;
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Farmaco aggiunto con successo.";
                return RedirectToAction("Index");
            }

            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "NomeAzienda", product.SupplierID);
            ViewBag.DrawerID = new SelectList(db.Drawers, "DrawerID", "DrawerID", product.DrawerID);
            return View(product);
        }

        public ActionResult Ricetta()
        {
            var redirect = RedirectToLoginIfNotAuthenticated();
            if (redirect != null) return redirect;
            return View();
        }

        [HttpPost]
        public ActionResult Ricetta(string MicrochipCodice)
        {
            var redirect = RedirectToLoginIfNotAuthenticated();
            if (redirect != null) return redirect;

            var beast = db.Beasts.FirstOrDefault(b => b.MicrochipCodice == MicrochipCodice);

            if (beast == null)
            {
                TempData["ErrorMessage"] = "Nessun animale trovato con questo codice microchip.";
                return View();
            }


            ViewBag.BeastID = beast.BeastID;


            var esami = db.Examinations
                          .Where(e => e.BeastID == beast.BeastID)
                          .OrderBy(e => e.DataVisita)
                          .ToList();

            return View(esami);
        }
        public ActionResult MedicinaXData()
        {
            var redirect = RedirectToLoginIfNotAuthenticated();
            if (redirect != null) return redirect;
            return View();
        }

        [HttpPost]
        public ActionResult GetMedicineByDate(DateTime dataVendita)
        {
            var redirect = RedirectToLoginIfNotAuthenticated();
            if (redirect != null) return redirect;
            var productIds = db.Sales
                                .Where(s => DbFunctions.TruncateTime(s.DataVendita) == dataVendita.Date)
                                .Select(s => s.ProductID)
                                .Distinct()
                                .ToList();

            var products = db.Products
                             .Where(p => productIds.Contains(p.ProductID))
                             .ToList();

            return PartialView("_MedicineByDate", products);
        }

        public ActionResult FarmacistaDet(int? id)
        {
            var redirect = RedirectToLoginIfNotAuthenticated();
            if (redirect != null) return redirect;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var farmacista = db.Pharmacists.FirstOrDefault(d => d.PharmacistID == id);
            if (farmacista == null)
            {
                return HttpNotFound();
            }

            return View(farmacista);
        }
        private ActionResult RedirectToLoginIfNotAuthenticated()
        {
            if (Session["UserF"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return null; 
        }


    }
}
