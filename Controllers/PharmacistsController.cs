using ClinicaVeterinaria.Models;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

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

    }
}
