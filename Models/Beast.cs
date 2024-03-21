namespace ClinicaVeterinaria.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Beast
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Beast()
        {
            Examinations = new HashSet<Examination>();
            Hospitalizations = new HashSet<Hospitalization>();
        }

        public int BeastID { get; set; }

        [Column(TypeName = "date")]
        public DateTime DataRegistrazione { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome { get; set; }

        [Required]
        [StringLength(50)]
        public string Tipologia { get; set; }

        [Required]
        [StringLength(50)]
        public string ColoreMantello { get; set; }

        [Required]
        [StringLength(400)]
        public string Foto { get; set; }

        [Column(TypeName = "date")]
        public DateTime DataNascita { get; set; }

        public bool Presunta { get; set; }

        public bool Microchip { get; set; }

        public bool Smarrito { get; set; }

        [StringLength(100)]
        public string Proprietario { get; set; }

        [StringLength(16)]
        public string CodiceFiscale { get; set; }

        [StringLength(50)]
        public string EmailProprietario { get; set; }

        [StringLength(20)]
        [RegularExpression(@"^\d{1,20}$", ErrorMessage = "Il campo CellulareProprietario deve contenere solo numeri.")]
        public string CellulareProprietario { get; set; }

        [StringLength(500)]
        public string PatologiePregresse { get; set; }

        [StringLength(10)]
        public string MicrochipCodice { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Examination> Examinations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Hospitalization> Hospitalizations { get; set; }
    }
}
