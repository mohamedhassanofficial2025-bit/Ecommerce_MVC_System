using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceSystem.DAL.Data.Models
{
    public class AppUser: IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}
