namespace University_of_free_state_booking_Facilities.Models.ViewModels
{
    public class DisplayViewModel
    {
        public IEnumerable<Booking> Booking { get; set; }
        public IEnumerable<Facility> Facilities { get; set; }

        public IEnumerable<Transaction> Transactions { get; set; }
    }

    public class FacilityInfoViewModel
    {
        public string FacilityName { get; set; }
        public int TotalBookings { get; set; }
        public decimal TotalAmount { get; set; }
    }

}
