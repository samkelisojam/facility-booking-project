using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using University_of_free_state_booking_Facilities.Models;

namespace University_of_free_state_booking_Facilities.Data
{
    public class BookingRepository :RepositoryBase<Booking>,IBookingRepository
    {
        public BookingRepository(AppDbContext appDbContext)
            : base(appDbContext)
        {
        }

    

       
      
    }
}
