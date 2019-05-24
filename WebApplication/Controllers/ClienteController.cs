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
        public IActionResult Index()
        {
            return View(ctx.RegistroClienteViewModel.ToList());
        }

        public IActionResult Registro()
        {
            return View();
        }
    }
}