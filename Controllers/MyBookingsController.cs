using System.Collections.Immutable;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Hotel.Data;
using Hotel.DTO;
using Hotel.Models;
using Hotel.Models.ViewModels;
using Hotel.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
    public class MyBookingsController : Controller
    {
        private readonly IMyBookingsService _service;

        public MyBookingsController(IMyBookingsService service)
        {
            _service = service;
        }

        [HttpGet("/MyBookings/Book")]
        public IActionResult Book()
        {
            var vm = new GuestBookingVm
            {
                NoOfPeople = 1,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1)
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(GuestBookingVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var dto = new GuestBookingDto(vm.Id, vm.NoOfPeople, vm.StartDate, vm.EndDate, vm.RoomNumber);

            var success = await _service.CreateMyBookingAsync(userId, dto);
            if (!success)
            {
                ModelState.AddModelError("", "No rooms available for this period.");
                return View(vm);
            }

            return RedirectToAction(nameof(MyBookings));
        }


        public async Task<IActionResult> MyBookings()
        {
            int id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var vms = await _service.GetMyBookingsAsync(id);

            return View(vms);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var vm = await _service.GetVmAsync(id);

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var success = await _service.DeleteMyBookingAsync(id, userId);
            if (!success) 
                return NotFound();

            return RedirectToAction(nameof(MyBookings));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserBookings(int id)
        {
            var vms = await _service.GetMyBookingsAsync(id);

            return View(vms);
        }
    }
}
