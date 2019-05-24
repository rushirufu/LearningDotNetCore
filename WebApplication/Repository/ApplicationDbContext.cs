using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Repository
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //use this to configure the contex
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pago> Pagos { get; set; }
    }


    #region Dominion De Entidades
    public class Cliente
    {
        // PK - Primary Key
        public int ClienteID { get; set; }
        public int DNI { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        // Propiedad De Navegacion
        public List<Pago> Pagos { get; set; }
    }

    public class Pago
    {
        // PK - Primary Key
        public int PagoID { get; set; } 
        public int Monto { get; set; }
        public DateTime Registro{ get; set; }
        // Fk - Foreign Key
        public int ClienteID { get; set; }
    }

    public class InfoContacto
    {
        // PK - Primary Key
        public int ClienteID { get; set; }
        public string Pais { get; set; }
        public string Estado { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
    }
    #endregion

    #region View Models

    public class RegistroClienteViewModel
    {
        [Required]
        public int PagoID { get; set; }
        public int Monto { get; set; }
    }
    #endregion
}
