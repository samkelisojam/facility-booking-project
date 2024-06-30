using GuitarShop.Data.DataAccess;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Web;
using System.Linq.Expressions;
using University_of_free_state_booking_Facilities.Data;
using University_of_free_state_booking_Facilities.Models;
using University_of_free_state_booking_Facilities.Models.ViewModels;
using Org.BouncyCastle.Crypto;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace University_of_free_state_booking_Facilities.Controllers
{
    [Authorize(Roles = "FacilityManager")]
    public class FacilityManagerController : Controller
    {


        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IRepositoryWrapper _repo;
        public int iPageSize = 4;
        public FacilityManagerController(IRepositoryWrapper Repo, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)

        {
            _repo = Repo;
            _signInManager = signInManager;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

     

        public IActionResult Index()
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

        private void UserPopulate()
        {

            var facilityInChargeUsers = _userManager.Users
                .Where(u => u.userType == "FacilityInCharge")
                .ToList();


            ViewBag.FacilityInChargeUsers = new SelectList(facilityInChargeUsers, "Id", "UserName");
        }


        public ActionResult ManageFacility()
        {

            var Facilities = _repo.Facility.FindAll().OrderBy(p => p.Name).ToList();


            return View(Facilities);

        }

        public ActionResult Cancel()
        {
            return View("Index");

        }


        public ActionResult ListTransactions()
        {
            var transactions = _repo.Transaction.FindAll().OrderBy(p => p.TransactionDate).ToList();
            return View(transactions);
        }




        [HttpGet]
        public ActionResult CreateFacility()
        {
            UserPopulate();
            return View();
        }
        [HttpPost]
        public ActionResult CreateFacility(Facility facility)
        {
            
            facility.ImageThumbnailUrl = "data:image/jpeg;base64,/9j/4AAQSkZJRgABAQAAAQABAAD/2wCEAAkGBw8NDQ0NDw0NDw0PDQ0NDQ0NDw8NDw0NFREWFhURFRUYHSggGBolHRUVITEhJSkrLi4uFx8zODMsNygtLisBCgoKDQ0NEg4PFSsZFRkrKysrKysrKysrKys3KystLSsrKysrKysrKy0tKysrKysrKysrKysrKysrKysrKysrK//AABEIAKwBJgMBIgACEQEDEQH/xAAXAAEBAQEAAAAAAAAAAAAAAAAAAQIH/8QAIRABAQEAAQMEAwAAAAAAAAAAAAERIWGBoQIxcfAiQVH/xAAUAQEAAAAAAAAAAAAAAAAAAAAA/8QAFBEBAAAAAAAAAAAAAAAAAAAAAP/aAAwDAQACEQMRAD8A7iAAACW+3Hf+KAAAAJJgKAAAAAAAAAAAAAAAAz6rZfTkt25bx+My3fEndoAAAMAAAAACgAAAAAAAAAAAAAAAAAAACbz7d1AAAAAAAAAAACgAAAAAAAAAaAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAMAAAAAAAAAAAAAAAAAAAAAAAAAAAAABL08qAAAAAEAAAAAAAAAAAAAAAAANABL96qAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAFAAAAAAAAABLAUAAEBQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAIAAAAAAAAAIoAAAAAAAAAAAACYoAAAACSqAAAAAAAAAAAAAAAAAAAAAAlgCgAAAAAJd/XHzN4UABKCgAAAAAAAAAAlBRMUAAAAAAAAEqgAAAAD/2Q==";
          
            facility.Bookings = new List<Booking>();

                _repo.Facility.Create(facility);

                _repo.Save();



            List<AppUser> users = _userManager.Users.ToList();
            

            foreach (AppUser item in users)
            {
                if (item.UserName == facility.InCharge) 
                {

                    item.facility = facility.Name;
                }
            }

            // Redirect to the facility listing page.
            return RedirectToAction("ManageFacility", "FacilityManager");

        }

        public ActionResult ApproveBooking(int bookingId)
        {

            Booking booking = _repo.Booking.GetById(bookingId);


            if (booking != null)
            {

                booking.IsConfirmed = true;

                _repo.Booking.Update(booking);
                _repo.Save();

                Notification notif = new Notification();
                notif.Title = "Approved Booking Facility";
                notif.Username = booking.UserName;
                notif.Description = "your booking has been approved to will be in :" + booking.BookingDate.ToShortDateString() + "\n" +
                    "facility Name is: " + booking.FacilityId + "\t will start at: " + booking.StartTime + " to " + booking.EndTime + "\n Thank you for booking with us enjoy our facility" +
                    "\n\nufs facility online+\n" + User.Identity.Name;
                _repo.Notification.Create(notif);
                _repo.Save();


                return RedirectToAction("Index");
            }


            return View();
        }

        [HttpGet]
        public IActionResult Reviews()
        {
            IEnumerable<FeedBack> feedBacks = _repo.Review.FindAll();

            return View(feedBacks);
        }
        [AllowAnonymous]
     
        public async Task<ActionResult> ResetPassword()
        {
            var user = await _userManager.GetUserAsync(User);
            PasswordViewModel passaword = new PasswordViewModel();
            passaword.Email = user.Email;


            return View(passaword);
        }





   

        [AllowAnonymous]
        [HttpPost]

        public async Task<ActionResult> ResetPassword(PasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(model.Password))
                {
                    if (await _userManager.HasPasswordAsync(user))
                    {
                        await _userManager.RemovePasswordAsync(user);
                    }

                    validPass = await _userManager.AddPasswordAsync(user, model.Password);

                    if (!validPass.Succeeded)
                    {
                        AddErrorsFromResult(validPass);
                    }
                }

                if ((validPass == null)
                      || (model.Password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await _userManager.UpdateAsync(user);



                    if (result.Succeeded)
                    {
                        string successMessage = $"Your password has been successfully change  {user.UserName} LogIn !!.";

                        // Store the message in TempData
                        TempData["ProfileEditedMessage"] = successMessage;

                        return RedirectToAction("LogIn", "Account");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }

            }



            return View(model);
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

            _repo.Facility.Delete(facility);
            _repo.Facility.Save();
            return RedirectToAction("ManageFacility");
        }


        public IActionResult Update(int id)
        {

            var facility = _repo.Facility.GetById(id);
            return View(facility);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, Facility facility)
        {



            // Update the MaxUsersAtOnce property
            var existingFacility = _repo.Facility.GetById(id);
            existingFacility.MaxUsersAtOnce = facility.MaxUsersAtOnce;
            _repo.Facility.Update(existingFacility);
            _repo.Facility.Save();


            return RedirectToAction("ManageFacility");


        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

    }
}
