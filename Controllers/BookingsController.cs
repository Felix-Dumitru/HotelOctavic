using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.DTO;
using Microsoft.AspNetCore.Authorization;
using Hotel.Models.ViewModels;
using Hotel.Service.Interfaces;

namespace Hotel.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BookingsController : Controller
    {
        private readonly IBookingService _service;

        public BookingsController(IBookingService bookingService)
        {
            _service = bookingService;
        }

        public async Task<IActionResult> Index()
        {
            var vms = await _service.GetAllVmsAsync();

            return View(vms);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookingVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var dto = new BookingDto
            (
                0,
                vm.UserName,
                vm.RoomNumber,
                vm.NoOfPeople,
                vm.StartDate,
                vm.EndDate
            );

            var result = await _service.CreateAsync(dto);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
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
        public async Task<IActionResult> Edit(int id, BookingVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var dto = new BookingDto
            (
                id,
                vm.UserName,
                vm.RoomNumber,
                vm.NoOfPeople,
                vm.StartDate,
                vm.EndDate
            );

            var result = await _service.UpdateAsync(id, dto);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(vm);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var vm = await _service.GetVmAsync(id);

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}