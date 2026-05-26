using EcommerceSystem.DAL.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace EcommerceSystem.PL.ViewModels.RoleManager
{
    public class RoleManagerVM
    {
        public string UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string? Role { get; set; }
    }
}
