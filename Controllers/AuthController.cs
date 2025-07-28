using System.Security.Claims;
using Hotel.Data;
using Hotel.Models;
using Hotel.Models.ViewModels.Auth;
using Hotel.Service.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService authService)
        {
            _auth = authService;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var (success, error) = await _auth.RegisterAsync(vm);

            if (!success)
            {
                ModelState.AddModelError("", error);
                return View(vm);
            }


            return RedirectToAction("Login");
        }

        public IActionResult Login(string? returnUrl = null)
        {
            return View(new LoginVm {ReturnUrl = returnUrl});
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var result = await _auth.BuildLoginAsync(vm.Email, vm.Password);


            if (!result.Success)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View(vm);
            }

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, result.Principal);

            return LocalRedirect(result.RedirectUrl ?? "/");

        }

        public async Task<IActionResult> Logout()
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
