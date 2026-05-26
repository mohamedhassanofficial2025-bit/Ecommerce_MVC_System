using EcommerceSystem.DAL.Data.Models;
using EcommerceSystem.PL.ViewModels.RoleManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceSystem.PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleManagerController : Controller
    {
        /*------------------------------------------------------------------*/
        private readonly UserManager<AppUser> _userManager;
        /*------------------------------------------------------------------*/

        public RoleManagerController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        /*------------------------------------------------------------------*/


        /*------------------------------------------------------------------*/

        public async Task<IActionResult> Index()
        {
            var users= _userManager.Users.ToList();

            var VMs= new List<RoleManagerVM>();
            foreach (var user in users) {

                var roles = await _userManager.GetRolesAsync(user);
                VMs.Add(new RoleManagerVM()
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email!,
                    Role = roles.Count > 0 ? roles[0] : "None"
                });
            }
            return View(VMs);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);

            // Remove all current roles first
            var currentRoles = await _userManager.GetRolesAsync(user!);
            await _userManager.RemoveFromRolesAsync(user!, currentRoles);

            // Assign new role if not "None"
            if (role != "None")
                await _userManager.AddToRoleAsync(user!, role);

            return RedirectToAction("Index");
        }
    }
}
