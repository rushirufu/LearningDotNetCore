using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication.Models;
using WebApplication.Repository;

namespace WebApplication.Repository
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClienteEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ClienteDatosDeContactoEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<ClienteDatosDeContacto> ClienteDatosDeContactos { get; set; }
    }

    #region Dominio De Entidades
    public class Cliente
    {
        public int Id { get; set; }     // PK - Primary Key
        public string DNI { get; set; } // Fk - Foreign Key Documento Nacional de Identidad
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        // Propiedad de navegacion
        public ClienteDatosDeContacto DatosDeContacto { get; set; }
    }
    public class ClienteDatosDeContacto
    {
        public int Id { get; set; }     // PK - Primary Key
        public string Pais { get; set; }
        public string Estado { get; set; }
        public string Direccion { get; set; }
        public int TelefonoLocal { get; set; }
        public int TelefonoCelular { get; set; }

        public int ClienteID { get; set; }  // Fk - Foreign Key   
        // Propiedad de navegacion
        public Cliente Cliente { get; set; } // Propiedad De Navegacion
    }
    #endregion

    internal class ClienteEntityTypeConfiguration : IEntityTypeConfiguration<Cliente>
    {
        private const string TableName = "Tabla_Clientes";
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable(TableName);
            builder.HasKey(cliente => cliente.Id);

            builder.Property(cliente => cliente.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(cliente => cliente.DNI).HasColumnName("dni").HasMaxLength(20).IsRequired();
            builder.Property(cliente => cliente.Nombre).HasColumnName("nombre").HasMaxLength(20).IsRequired();
            builder.Property(cliente => cliente.Apellido).HasColumnName("apellido").HasMaxLength(20).IsRequired();
            // Relaciones
            builder.HasOne(relacion => relacion.DatosDeContacto) // Propiedad de navegacion
                   .WithOne(relacion => relacion.Cliente)
                   .HasForeignKey<ClienteDatosDeContacto>(relacion => relacion.ClienteID);
        }
    }

    internal class ClienteDatosDeContactoEntityTypeConfiguration : IEntityTypeConfiguration<ClienteDatosDeContacto>
    {
        private const string TableName = "Tabla_ClienteDatosDeContacto";
        public void Configure(EntityTypeBuilder<ClienteDatosDeContacto> builder)
        {
            builder.ToTable(TableName);
            builder.HasKey(datosContacto => datosContacto.Id);

            builder.Property(datosContacto => datosContacto.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(datosContacto => datosContacto.Pais).HasColumnName("pais").HasMaxLength(20).IsRequired();
            builder.Property(datosContacto => datosContacto.Estado).HasColumnName("estado").HasMaxLength(20).IsRequired();
            builder.Property(datosContacto => datosContacto.Direccion).HasColumnName("direccion").HasMaxLength(20).IsRequired();
            builder.Property(datosContacto => datosContacto.TelefonoLocal).HasColumnName("telefono_local").HasMaxLength(20).IsRequired();
            builder.Property(datosContacto => datosContacto.TelefonoCelular).HasColumnName("telefono_celular").HasMaxLength(20).IsRequired();
        }
    }

    public class RegistroClienteViewModel
    {
        [NotMapped]
        public int Id { get; set; }
        [Display(Name = "Documento nacional de Identidad"),]
        public string DNI { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Pais { get; set; }
        public string Estado { get; set; }
        public string Direccion { get; set; }
        public int TelefonoLocal { get; set; }
        public int TelefonoCelular { get; set; }
    }
}
