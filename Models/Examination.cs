namespace ClinicaVeterinaria.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Examination
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Examination()
        {
            Sales = new HashSet<Sale>();
        }

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
