using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Repository;

namespace WebApplication.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ApplicationDbContext ctx;
        public ClienteController(ApplicationDbContext context)
        {
            ctx = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(ctx.Clientes.ToList());
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registro(RegistroClienteViewModel vm)
        {

            if (ctx.Clientes.Any(u => u.DNI == vm.DNI))
            {
                var ValidacionDNI = "El DNI Ya Existe";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    Cliente ModeloCliente = new Cliente()
                    {
                        DNI = vm.DNI,
                        Nombre = vm.Nombre,
                        Apellido = vm.Apellido,
                    };

                    ClienteDatosDeContacto datosContacto = new ClienteDatosDeContacto()
                    {
                        Pais = vm.Pais,
                        Estado = vm.Estado,
                        Direccion = vm.Direccion,
                        TelefonoLocal = vm.TelefonoLocal,
                        TelefonoCelular = vm.TelefonoCelular 
                    };
                    ModeloCliente.DatosDeContacto = datosContacto; // Propiedad de navegacion

                    ctx.Add(ModeloCliente);
                    ctx.SaveChanges();
                }
            }
            return View();
        }
    }
}