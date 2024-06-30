using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University_of_free_state_booking_Facilities.Data;
using University_of_free_state_booking_Facilities.Models;
using University_of_free_state_booking_Facilities.Models.ViewModels;

namespace University_of_free_state_booking_Facilities.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IRepositoryWrapper _repo;
        public UsersController(IRepositoryWrapper repo)
        {
            _repo = repo;
        }

     

        public IActionResult NotificationMessage()
        {
            var notifications = _repo.Notification.FindAll().Where(p => p.Username == User.Identity.Name).ToList();
            return View(notifications);
        }
        public IActionResult MyBooking()
        {
   
            var allFacilities = _repo.Facility.FindAll().ToList();

            var facilityBookingsPairs = new List<FacilityBookingViewModel>();

       
            foreach (var facility in allFacilities)
            {
                var bookingsForFacility = _repo.Booking.FindAll()
                    .Where(b => b.FacilityId == facility.FacilityId && b.UserName == User.Identity.Name)
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


        [HttpGet]
        public IActionResult ReschenduleBooking(int bookingId)
        {
          
            var booking = _repo.Booking.GetById(bookingId);

            if (booking == null)
            {
                return NotFound(); 
            }
            @ViewBag.username = User.Identity.Name;
            @ViewBag.Cat =booking.FacilityId.ToString();
    
            return View("ReschenduleBooking", booking);
        }

        [HttpPost]
        public IActionResult ReschenduleBooking(Booking updatedBooking) 
        {
            if (ModelState.IsValid)
            {
           
                var existingBooking = _repo.Booking.GetById(updatedBooking.BookingId);

                if (existingBooking == null)
                {
                    return NotFound(); 
                }

                existingBooking.BookingDate = updatedBooking.BookingDate;
                existingBooking.StartTime = updatedBooking.StartTime;
                existingBooking.EndTime = updatedBooking.EndTime;
                existingBooking.IsConfirmed = false;
                _repo.Booking.Update(existingBooking);
                _repo.Save();

                return RedirectToAction("MyBooking"); 
            }

         
            return View("ReschenduleBooking", updatedBooking);
        }


    }
}
