namespace ClinicaVeterinaria.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Examination
    {

        public int ExaminationID { get; set; }

        [Column(TypeName = "date")]
        public DateTime DataVisita { get; set; }

        [Required]
        public string PatologieRiscontrate { get; set; }

        [Required]
        [StringLength(8)]
        public string NumeroRicetta { get; set; }

        [Required]
        public string Ricetta { get; set; }

        public decimal Prezzo { get; set; }

        public int DoctorID { get; set; }

        public int BeastID { get; set; }

        public virtual Beast Beast { get; set; }

        public virtual Doctor Doctor { get; set; }

    }
}
