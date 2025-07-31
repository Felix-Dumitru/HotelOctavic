using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.DTO;
using Hotel.Models;
using Hotel.Models.ViewModels;
using Hotel.Service.Interfaces;


namespace Hotel.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _service;
        public UsersController(IUserService service)
        {
            _service  = service;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _service.GetAllVmsAsync();
            return View(user);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var dto = new UserDto
            (
                0,
                vm.Name,
                vm.Email,
                vm.Password,
                vm.Role
            );

            var user = await _service.CreateAsync(dto);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid user data.");
                return View(vm);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _service.GetVmAsync(id);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UserVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var dto = new UserDto
                (
                id,
                vm.Name,
                vm.Email,
                vm.Password,
                vm.Role
            );

            var result = await _service.UpdateAsync(id, dto);

            if (result == null)
            {
                ModelState.AddModelError("Invalid user data", "Invalid user data.");
                return View(vm);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _service.GetVmAsync(id);
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _service.DeleteAsync(id);
            return RedirectToAction("Index", "Users");
        }
    }
}