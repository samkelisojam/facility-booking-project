using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using University_of_free_state_booking_Facilities.Models;

namespace University_of_free_state_booking_Facilities.Data
{
    public class Users
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Facility { get; set; }

    }

    public static class SeedIdentityData
    {
       
      
            private static readonly List<Users> ListofUser = new()
        {


              new Users {Username = "Randy",Email="Randy@gmail.com", Role = "FacilityManager", Facility= "" },
                new Users {Username = "Kelly",Email="Kelly@gmail.com", Role = "FacilityManager", Facility= "" },
                new Users {Username ="Nonhle",Email="Nonhle@gmail.com",Role="FacilityInCharge",Facility="StudyHall"},
                   new Users {Username = "Katlego",Email="katlego@gmail.com", Role = "FacilityInCharge", Facility= "Parking" },
                 new Users {Username = "Muzi",Email="Muzi@gmail.com", Role = "FacilityInCharge", Facility= "Gym" },
                  new Users {Username = "Elihle",Email="Elihle@gmail.com", Role = "FacilityInCharge",Facility="Laundry" }



        };
        public static async void EnsureIdentityPopulated(IApplicationBuilder app)
        {
            AppIdentityDbContext context = app.ApplicationServices
                         .CreateScope().ServiceProvider
                         .GetRequiredService<AppIdentityDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            UserManager<AppUser> userManager = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<UserManager<AppUser>>();

            RoleManager<IdentityRole> roleManager = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            foreach (Users userMember in ListofUser)
            {


                if (await userManager.FindByEmailAsync(userMember.Email) == null)
                {
                    if (await roleManager.FindByNameAsync(userMember.Role) == null)
                    {
                        await roleManager.CreateAsync(new IdentityRole(userMember.Role));
                    }

                    AppUser user = new()
                    {
                        UserName = userMember.Username,
                        Email = userMember.Email,
                        ConfirmPassword = userMember.Username + "@2023",
                        squestion1 = "none",
                        sQuestion2 = "none",
                        sQuestion3 = "none",
                        userType = userMember.Role,
                        FirstName = userMember.Username,
                        LastName = userMember.Username,
                        IdentityNumber = "0000",
                        facility = userMember.Facility
                    };

                    IdentityResult result = await userManager.CreateAsync(user,userMember.Username+"@2023");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, userMember.Role);
                    }

                }
            }
        }

        }
    }
