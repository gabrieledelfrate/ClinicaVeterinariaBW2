namespace ClinicaVeterinaria.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Doctor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Doctor()
        {
            Examinations = new HashSet<Examination>();
            Hospitalizations = new HashSet<Hospitalization>();
        }

        public int DoctorID { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome { get; set; }

        [Required]
        [StringLength(50)]
        public string Cognome { get; set; }

        [Column(TypeName = "date")]
        public DateTime DataNascita { get; set; }

        [Required]
        [StringLength(100)]
        public string LuogoNascita { get; set; }

        [Required]
        [StringLength(100)]
        public string LuogoResidenza { get; set; }

        [Required]
        [StringLength(16)]
        public string CodiceFiscale { get; set; }

        [Required]
        [StringLength(5)]
        public string Matricola { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string PasswordMedico { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Examination> Examinations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Hospitalization> Hospitalizations { get; set; }
    }
}
