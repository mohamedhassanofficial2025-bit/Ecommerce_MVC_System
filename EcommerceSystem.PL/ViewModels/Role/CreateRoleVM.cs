using System.ComponentModel.DataAnnotations;

namespace ASP.NETCoreD10.ViewModels.Role
{
    public class CreateRoleVM
    {
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public required string Name { get; set; }
    }
}
