using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class DebugController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var ws = new WebClient();
            var UrlAddress = ws.DownloadString("https://randomuser.me/api/?results=20");
            var resultado = JsonConvert.DeserializeObject<RootObject>(UrlAddress);
            return View(resultado.results);
        }

        //[HttpPost]
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}