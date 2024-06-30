using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Macs;
using System.Text;
using University_of_free_state_booking_Facilities.Data;
using University_of_free_state_booking_Facilities.Models;
using University_of_free_state_booking_Facilities.Models.ViewModels;

namespace University_of_free_state_booking_Facilities.Controllers
{

    [Authorize(Roles = "FacilityAdmin")]
    public class FacilityAdminController : Controller
    {
        
        private readonly SignInManager<AppUser> _signInManager;


        private readonly UserManager<AppUser> _userManager;
      
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppIdentityDbContext _context;

        private readonly IRepositoryWrapper _repo;
        public FacilityAdminController(IRepositoryWrapper repo, UserManager<AppUser> userManager, AppIdentityDbContext context, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _repo = repo;
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
     

        public IActionResult List()
        {
            var facility = _repo.Facility.FindAll()
                .OrderBy(c => c.Name).ToList();
            return View(facility);
        }

        public async Task< IActionResult> ListFacilityInCharge()
        {
           
            IEnumerable<Facility> facilities = _repo.Facility.FindAll();
            IQueryable <AppUser> Users =  _userManager.Users;
            List<AppUser> Users2 = new List<AppUser>();
            foreach (var item in  facilities)
            {
                foreach (var user in Users)
                {

                    if (item.InCharge == user.UserName  )
                    {
                        Users2.Add(user);
                    }
                }
                

            }
        
         
            return View(Users2);
        }
        public IActionResult ListUsers()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }
        //

        public ActionResult OrdersBooking()
        {
     
            var allFacilities = _repo.Facility.FindAll().ToList();

       
            var facilityBookingsPairs = new List<FacilityBookingViewModel>();

          
            foreach (var facility in allFacilities)
            {
                var bookingsForFacility = _repo.Booking.FindAll()
                    .Where(b => b.FacilityId == facility.FacilityId)
                    .ToList();

                var facilityBookingPair = new FacilityBookingViewModel
                {
                    Facility = facility,
                    Bookings = bookingsForFacility
                };

                facilityBookingsPairs.Add(facilityBookingPair);
            }


            return View(facilityBookingsPairs);
        }

        public IActionResult Update(int id)
        {
           
            var facility = _repo.Facility.GetById(id);
            return View(facility);
        }
		[HttpGet]
		public IActionResult PrintGenerateReport()
		{
			try
			{

				IQueryable<AppUser> users = _userManager.Users;


				var reportText = "Name\t\tSurname\t\tEmail\t\tRole\t\tUserType";


				foreach (AppUser user in users)
				{
					reportText += $"\n{user.UserName}\t\t{user.LastName}\t\t{user.Email}\t\t{user.userType}";
				}


				var fileName = "All_Information.pdf";

				var reportBytes = Encoding.UTF8.GetBytes(reportText);


				return File(reportBytes, "text/plain", fileName);
			}
			catch (Exception ex)
			{

				Console.WriteLine(ex.Message);
				return BadRequest("An error occurred during report generation.");
			}
		}

		[HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Update(int id, Facility facility)
        {
           

         
           
                    var existingFacility = _repo.Facility.GetById(id);
                    existingFacility.MaxUsersAtOnce = facility.MaxUsersAtOnce;
                    _repo.Facility.Update(existingFacility);
                    _repo.Facility.Save();
                
               
                return RedirectToAction("List"); 
        
           
        }

        public IActionResult Delete(int id)
        {
      
            var facility = _repo.Facility.GetById(id);
            return View(facility);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
     
            var facility = _repo.Facility.GetById(id);
            facility.MaxUsersAtOnce = 0; 
            _repo.Facility.Update(facility);
            _repo.Facility.Save();
            return RedirectToAction("List"); 
        }

		public ActionResult GenerateReport()
		{
			List<FacilityInfoViewModel> facilityInfoList = _repo.Facility.GetFacilityInfo();

			return View(facilityInfoList);
		}
        [HttpPost]
		public IActionResult DeleteUser(string userId)
		{
			if (string.IsNullOrEmpty(userId))
			{
				return BadRequest("User ID is required.");
			}

			var user = _userManager.FindByIdAsync(userId).Result;

			if (user == null)
			{
				return NotFound("User not found.");
			}

			var result = _userManager.DeleteAsync(user).Result;

			if (result.Succeeded)
			{
				return RedirectToAction("ListUsers");
			}
			else
			{
				return View("Error");
			}
		}

		public IActionResult ActivateUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required.");
            }

            var user = _userManager.FindByIdAsync(userId).Result;

            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.LockoutEnabled = false;
            user.LockoutEnd = null; // Clear the lockout end date to activate the user

            var result = _userManager.UpdateAsync(user).Result;

            if (result.Succeeded)
            {
                return RedirectToAction("List");
            }
            else
            {
                return View("Error");
            }
        }



        public IActionResult RegisterFacilityStaff() 
        {
        return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterFacilityStaff(RegisterStaff model)
        {
            if (ModelState.IsValid)
            {

                AppUser user = new()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.FirstName + "@ufs4facilitybooking.ac.za",
                    IdentityNumber = model.IdentityNumber,
                    userType = model.UserType,

                    ConfirmPassword = model.FirstName + "@2023",
                    UserName = model.FirstName,

                    squestion1 ="null",
                    sQuestion2 = "null",
                    sQuestion3 = "null",

                };


                
                var result = await _userManager.CreateAsync(user, model.FirstName+"@2023");

                if (result.Succeeded)
                {
                  
                    return RedirectToAction("ListUsers");
                }
                else
                {
                    
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

         
            return View(model);
        }
     


    }
}
