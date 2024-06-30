using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Net;
using System.Text.RegularExpressions;
using University_of_free_state_booking_Facilities.Data;
using University_of_free_state_booking_Facilities.Models;
using University_of_free_state_booking_Facilities.Models.ViewModels;

namespace University_of_free_state_booking_Facilities.Controllers
{
    [Authorize(Roles = "FacilityInCharge")]
    public class FacilityInChargeController : Controller
    {
        private readonly IRepositoryWrapper _repo;
        public FacilityInChargeController(IRepositoryWrapper repo)
        {
            _repo = repo;
        }
        public IActionResult BookingDetails()
        {
            string userInCharge = User.Identity.Name;

           
            Facility facility = _repo.Facility.FindAll()
                .Where(f => f.InCharge == userInCharge)
                .FirstOrDefault();

            if (facility != null)
            {
                ViewBag.FacilityName = facility.Name;

               
                var bookingInCharge = _repo.Booking.FindAll()
                    .Where(b => b.FacilityId == facility.FacilityId)
                    .ToList();

                return View(bookingInCharge);
            }
            else
            {
                ViewBag.MessIncharge = "There are no bookings for this Facility";
                return View();
            }
            return View();
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


                return RedirectToAction("BookingDetails");
            }


            return View();
        }


    }
}
