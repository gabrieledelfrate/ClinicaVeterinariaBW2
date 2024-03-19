namespace ClinicaVeterinaria.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Uses")]
    public partial class Us
    {
        [Key]
        public int UseID { get; set; }

        [Required]
        [StringLength(50)]
        public string Descrizione { get; set; }

        public int ProductID { get; set; }

        public virtual Product Product { get; set; }
    }
}
