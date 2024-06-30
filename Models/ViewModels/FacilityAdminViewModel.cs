namespace University_of_free_state_booking_Facilities.Models.ViewModels
{
  
    public class UserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
        public bool ShowMessageIcon { get; set; }

    }
    public class WriteMessageViewModel
    {
        public string ReceiverEmail { get; set; }
        public string Content { get; set; }
    }
    public class FacilityBookingViewModel
    {
        public Facility Facility { get; set; }
        public List<Booking> Bookings { get; set; }
    }
    public class AssingInChargeViewModel
    {
        public string inCharge { get; set; }
      
    }

}
