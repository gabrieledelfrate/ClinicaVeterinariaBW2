namespace ClinicaVeterinaria.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Hospitalization
    {
        public int HospitalizationID { get; set; }

        [Column(TypeName = "date")]
        public DateTime DataInizioRicovero { get; set; }

        [Required]
        [StringLength(100)]
        public string Prognosi { get; set; }

        public int BeastID { get; set; }

        public int DoctorID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DataFineRicovero  { get; set; }
        public decimal CostoGiornalieroRicovero { get; set; }

        public virtual Beast Beast { get; set; }

        public virtual Doctor Doctor { get; set; }
    }
}
