using ASP.NETCoreD10.ViewModels.Role;
using EcommerceSystem.DAL.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EcommerceSystem.PL.Controllers
{

    public class RoleController : Controller
    {
        /*------------------------------------------------------------------*/
        private readonly RoleManager<AppRole> _roleManager;
        /*------------------------------------------------------------------*/

        public RoleController(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        /*------------------------------------------------------------------*/
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        /*------------------------------------------------------------------*/
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(CreateRoleVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var Role = new AppRole()
            {
                Name = vm.Name,
            };

            //create role
            IdentityResult result = await _roleManager.CreateAsync(Role);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(vm);
            }
            return RedirectToAction("Index","Home");
        }

    }
}
