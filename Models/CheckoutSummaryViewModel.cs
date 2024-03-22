using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClinicaVeterinaria.Models
{
    public class CheckoutSummaryViewModel
    {
        public Beast Animale { get; set; }
        public List<Sale> Vendite { get; set; }
        public decimal PrezzoTotale { get; set; }
        public string NumeroRicetta { get; set; }
        public DateTime DataVendita { get; set; }
        public int Quantita { get; set; }
        public int PharmacistID { get; set; }
        public string NomeFarmacista { get; set; }
    }
}