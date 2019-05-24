using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int PropertyA { get; set; }
        public int MyPropertyB { get; set; }
    }
}
