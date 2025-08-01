﻿using Hotel.Data;
using Hotel.Models;
using Hotel.Models.ViewModels;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Hotel.DTO;
using Microsoft.EntityFrameworkCore;
using Hotel.Service.Interfaces;

namespace Hotel.Service
{
    public class MyBookingsService : IMyBookingsService
    {
        private readonly HotelContext _context;

        public MyBookingsService(HotelContext context)
        {
            _context = context;
        }

        public async Task<List<BookingVm>> GetMyBookingsAsync(int userId)
        {
            var vms = await _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.Room)
                .Select(b => new BookingVm
                {
                    Id = b.Id,
                    UserName = b.User.Name,
                    RoomNumber = b.Room.Number,
                    NoOfPeople = b.NoOfPeople,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate
                })
                .ToListAsync();

            return vms;
        }

        public async Task<bool> CreateMyBookingAsync(int userId, GuestBookingDto dto)
        {
            var goodRooms = _context.Rooms
                .Where(r => r.Capacity >= dto.NoOfPeople)
                .Where(r => !_context.Bookings.Any(b =>
                    b.RoomId == r.Id &&
                    dto.StartDate < b.EndDate &&
                    dto.EndDate > b.StartDate));

            if (!string.IsNullOrWhiteSpace(dto.RoomNumber))
            {
                var typed = dto.RoomNumber.Trim();
                goodRooms = goodRooms.Where(r => r.Number == typed);
            }

            var freeRoom = await goodRooms.FirstOrDefaultAsync();

            if (freeRoom == null)
                return false;

            Booking booking = new Booking
            {
                UserId = userId,
                RoomId = freeRoom.Id,
                NoOfPeople = dto.NoOfPeople,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<GuestBookingVm?> GetVmAsync(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (booking == null)
                return null;
            return new GuestBookingVm
            {
                Id = booking.Id,
                NoOfPeople = booking.NoOfPeople,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                RoomNumber = booking.Room.Number
            };
        }

        public async Task<bool> DeleteMyBookingAsync(int id, int userId)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);
            if (booking == null) return false;

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
