using Microsoft.EntityFrameworkCore;
using University_of_free_state_booking_Facilities.Models;

namespace University_of_free_state_booking_Facilities.Data
{
    public static class SeedData
    {
        public static void EnsureEntityPopulated(IApplicationBuilder app)
        {
            AppDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            if (!context.Facilities.Any())
            {
                context.Facilities.AddRange(
                    new Facility { Name = "Gym", Description = "sdfg", Price = 10m,InCharge="Muzi" ,MaxUsersAtOnce=350, ImageThumbnailUrl= "https://media.istockphoto.com/id/1132006407/photo/empty-gym.jpg?s=1024x1024&w=is&k=20&c=iGC9lJUQOT4copJjwL9PCf41Eu7bn9y0hdkdinqwoac=" }, //CategoryID = 1
                    new Facility { Name = "Laundry", Description = "sdfg", Price = 10m,InCharge= "Elihle", MaxUsersAtOnce = 35 , ImageThumbnailUrl = "https://media.istockphoto.com/id/169982509/photo/washing-machines-clothes-washers-door-in-a-public-launderette.jpg?s=1024x1024&w=is&k=20&c=HWN6NOyAj91GzD2_W-R04yZk5Z9btjUNyOAw-9h7AXQ=" }, //CategoryID = 2
                    new Facility { Name = "StudyHall", Description = "", Price = 10m, InCharge = "Nonhle", MaxUsersAtOnce = 35, ImageThumbnailUrl = "https://media.istockphoto.com/id/1667072269/photo/lecture-hall.jpg?s=1024x1024&w=is&k=20&c=5uKyBteO0eEH3qqO78V6Qd9j07OJTD4ExYcniSK3adI=" } ,//CategoryID = 3
                    new Facility { Name = "Parking", Description = "student Parking", Price = 10m, InCharge = "Katlego", MaxUsersAtOnce = 35, ImageThumbnailUrl = "https://media.istockphoto.com/id/532186572/photo/cars-in-the-parking-lot-in-row.jpg?s=1024x1024&w=is&k=20&c=CHCpM7HvcHTaHgVVqtRQtUAXefj_gpn_0F52O35KU2U=" } //CategoryID = 3

                    );
            }
            context.SaveChanges();
        }
    }
}
