using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Service;
using Microsoft.AspNetCore.Authorization;
using Hotel.Models.ViewModels;

namespace Hotel.Controllers
{
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

            var success = await _service.CreateAsync(vm);
            if (success == null)
            {
                ModelState.AddModelError("", "Error with user, room or dates.");
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

            var success = await _service.UpdateAsync(id, vm);
            if (success == false)
            {
                ModelState.AddModelError("", "Error with user, room or dates.");
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