using System.ComponentModel.DataAnnotations;

namespace EcommerceSystem.PL.ViewModels.Auth
{
    public class RegisterVM
    {
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public required string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public required string LastName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public required string ConfirmPassword { get; set; }
    }
}
