using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace ClinicaVeterinaria.Models
{
    public partial class DBContext : DbContext
    {
        public DBContext()
            : base("name=DBContext")
        {
        }

        public virtual DbSet<Beast> Beasts { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Drawer> Drawers { get; set; }
        public virtual DbSet<Examination> Examinations { get; set; }
        public virtual DbSet<Hospitalization> Hospitalizations { get; set; }
        public virtual DbSet<Locker> Lockers { get; set; }
        public virtual DbSet<Pharmacist> Pharmacists { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<Us> Uses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Beast>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<Beast>()
                .Property(e => e.Tipologia)
                .IsUnicode(false);

            modelBuilder.Entity<Beast>()
                .Property(e => e.ColoreMantello)
                .IsUnicode(false);

            modelBuilder.Entity<Beast>()
                .Property(e => e.Proprietario)
                .IsUnicode(false);

            modelBuilder.Entity<Beast>()
                .Property(e => e.CodiceFiscale)
                .IsUnicode(false);

            modelBuilder.Entity<Beast>()
                .Property(e => e.EmailProprietario)
                .IsUnicode(false);

            modelBuilder.Entity<Beast>()
                .Property(e => e.CellulareProprietario)
                .IsUnicode(false);

            modelBuilder.Entity<Beast>()
                .HasMany(e => e.Examinations)
                .WithRequired(e => e.Beast)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Beast>()
                .HasMany(e => e.Hospitalizations)
                .WithRequired(e => e.Beast)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Doctor>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<Doctor>()
                .Property(e => e.Cognome)
                .IsUnicode(false);

            modelBuilder.Entity<Doctor>()
                .Property(e => e.LuogoNascita)
                .IsUnicode(false);

            modelBuilder.Entity<Doctor>()
                .Property(e => e.LuogoResidenza)
                .IsUnicode(false);

            modelBuilder.Entity<Doctor>()
                .Property(e => e.CodiceFiscale)
                .IsUnicode(false);

            modelBuilder.Entity<Doctor>()
                .Property(e => e.Matricola)
                .IsUnicode(false);

            modelBuilder.Entity<Doctor>()
                .Property(e => e.PasswordMedico)
                .IsUnicode(false);

            modelBuilder.Entity<Doctor>()
                .HasMany(e => e.Examinations)
                .WithRequired(e => e.Doctor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Doctor>()
                .HasMany(e => e.Hospitalizations)
                .WithRequired(e => e.Doctor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Drawer>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.Drawer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Examination>()
                .Property(e => e.NumeroRicetta)
                .IsUnicode(false);

            modelBuilder.Entity<Examination>()
                .Property(e => e.Prezzo)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Hospitalization>()
                .Property(e => e.Prognosi)
                .IsUnicode(false);

            modelBuilder.Entity<Hospitalization>()
                .Property(e => e.CostoGiornalieroRicovero)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Locker>()
                .Property(e => e.Descrizione)
                .IsUnicode(false);

            modelBuilder.Entity<Pharmacist>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<Pharmacist>()
                .Property(e => e.Cognome)
                .IsUnicode(false);

            modelBuilder.Entity<Pharmacist>()
                .Property(e => e.LuogoNascita)
                .IsUnicode(false);

            modelBuilder.Entity<Pharmacist>()
                .Property(e => e.LuogoResidenza)
                .IsUnicode(false);

            modelBuilder.Entity<Pharmacist>()
                .Property(e => e.CodiceFiscale)
                .IsUnicode(false);

            modelBuilder.Entity<Pharmacist>()
                .Property(e => e.Matricola)
                .IsUnicode(false);

            modelBuilder.Entity<Pharmacist>()
                .Property(e => e.PasswordFarmacista)
                .IsUnicode(false);

            modelBuilder.Entity<Pharmacist>()
                .HasMany(e => e.Sales)
                .WithRequired(e => e.Pharmacist)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Descrizione)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Sales)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Uses)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sale>()
                .Property(e => e.CodiceFiscale)
                .IsUnicode(false);

            modelBuilder.Entity<Sale>()
                .Property(e => e.Prezzo)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.NomeAzienda)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.Indirizzo)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.Telefono)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.Supplier)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Us>()
                .Property(e => e.Descrizione)
                .IsUnicode(false);
        }
    }
}
