namespace ClinicaVeterinaria.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Sale
    {
        public int SaleID { get; set; }

        public int ProductID { get; set; }

        [Required]
        [StringLength(16)]
        public string CodiceFiscale { get; set; }
        [StringLength(8)]
        public string NumeroRicetta { get; set; }

        public decimal Prezzo { get; set; }

        public int PharmacistID { get; set; }

        public DateTime DataVendita { get; set; }

        [Required]
        public int Quantita { get; set; }

        public virtual Pharmacist Pharmacist { get; set; }

        public virtual Product Product { get; set; }
    }
}
