using Hotel.Data;
using Hotel.DTO;
using Hotel.Models;
using Hotel.Models.ViewModels;
using Hotel.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Hotel.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoomsController : Controller
    {
        private readonly IRoomService _service;
        public RoomsController(IRoomService service)
        {
            _service = service;
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
        public async Task<IActionResult> Create(RoomVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var dto = new RoomDto
            (
                0,
                vm.Number,
                vm.Capacity
            );

            var room = await _service.CreateAsync(dto);

            if (room == null)
            {
                ModelState.AddModelError("", "Invalid room data.");
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
        public async Task<IActionResult> Edit(int id, RoomVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var dto = new RoomDto
            (
                id,
                vm.Number,
                vm.Capacity
            );

            var result = await _service.UpdateAsync(id, dto);

            if (result == null)
            {
                ModelState.AddModelError("", "Invalid room data.");
                return View(vm);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var room = await _service.GetVmAsync(id);
            return View(room);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _service.DeleteAsync(id);

            return RedirectToAction("Index", "Rooms");
        }

        [HttpGet("/api/rooms/bookeddates/{roomId:int}")]
        public async Task<IActionResult> GetBookedDates(int id)
        {
            var days = await _service.GetBookedDatesAsync(id);

            return Ok(days.Distinct());
        }

        public async Task<IActionResult> Calendar(int id)
        {
            var vm = await _service.GetCalendarVmAsync(id);

            if(vm == null)
                return NotFound();

            return View(vm);
        }
    }
}
