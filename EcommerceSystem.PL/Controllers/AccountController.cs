using EcommerceSystem.DAL.Data.Models;
using EcommerceSystem.PL.SystemRoles;
using EcommerceSystem.PL.ViewModels.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace EcommerceSystem.PL.Controllers
{
    public class AccountController : Controller
    {
        /*--------------------------------------------------------*/
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        /*--------------------------------------------------------*/
        [HttpGet]
        #region Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Register(RegisterVM VM)
        {
            if (!ModelState.IsValid)
            {
                return View(VM);
            }

            AppUser appUser = new AppUser()
            {
                FirstName = VM.FirstName,
                LastName = VM.LastName,
                Email = VM.Email,
                UserName= VM.Email,
            };

            //add to database
            var resultCreateUser = await _userManager.CreateAsync(appUser,VM.Password);
            //check result
            if (!resultCreateUser.Succeeded)
            {
                //error handling
                foreach(var error in resultCreateUser.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(VM);
            }
            //add role
            var resultAddRole = await _userManager.AddToRoleAsync(appUser, SysRoles.UserRole);

            if (!resultAddRole.Succeeded)
            {
                //error handling
                foreach (var error in resultAddRole.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(VM);
            }
            return RedirectToAction(nameof(Login));
        }
        #endregion
        /*--------------------------------------------------------*/

        #region Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(LoginVM VM)
        {
            if (!ModelState.IsValid)
            {
                return View(VM);
            }
            AppUser? appUser =await _userManager.FindByEmailAsync(VM.Email);
            if(appUser == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Email Or Password!");
                return View(VM);
            }

            var result = await _signInManager.PasswordSignInAsync(appUser, VM.Password, VM.RememberMe, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid Email Or Password!");
                return View(VM);
            }

            return RedirectToAction("Index", "Home");
        }
        #endregion
        /*------------------------------------------------------------------*/
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        /*------------------------------------------------------------------*/
    }
}
