using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using University_of_free_state_booking_Facilities.Models;

namespace University_of_free_state_booking_Facilities.Data
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
       
        public AppIdentityDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
