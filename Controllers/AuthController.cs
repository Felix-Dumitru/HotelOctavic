using Hotel.Data;
using Hotel.DTO;
using Hotel.DTO.Auth;
using Hotel.Models;
using Hotel.Models.ViewModels.Auth;
using Hotel.Service.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hotel.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService authService)
        {
            _auth = authService;
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var dto = new RegisterRequestDto(vm.Email, vm.Password, vm.Role);
            var (success, error) = await _auth.RegisterAsync(dto);

            if (!success)
            {
                ModelState.AddModelError("", error);
                return View(vm);
            }


            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            return View(new LoginVm {ReturnUrl = returnUrl});
        }

        [AllowAnonymous, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var dto = new LoginRequestDto(vm.Email, vm.Password);
            var result = await _auth.BuildLoginAsync(dto);


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

        [AllowAnonymous]
        public IActionResult Denied(string? returnUrl = null)
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
                returnUrl = Request.Headers["Referer"].ToString();

            if (!Url.IsLocalUrl(returnUrl))
                returnUrl = Url.Content("~/");

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
    }
}
