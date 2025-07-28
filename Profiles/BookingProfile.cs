using AutoMapper;
using Hotel.Models;
using Hotel.Models.ViewModels;

namespace Hotel.Profiles
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<Booking, BookingVm>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.User.Name))
                .ForMember(d => d.RoomNumber, o => o.MapFrom(s => s.Room.Number));
        }
    }

}
