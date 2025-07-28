using Hotel.Data;

namespace Hotel.Service
{
    public class UserService
    {
        private readonly HotelContext _context;

        public UserService(HotelContext context)
        {
            _context = context;
        }


    }
}