using Microsoft.EntityFrameworkCore;

using University_of_free_state_booking_Facilities.Models;
using University_of_free_state_booking_Facilities.Models.ViewModels;

namespace University_of_free_state_booking_Facilities.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
     
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
     
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<FeedBack> Feedacks { get; set; }

    }
}
