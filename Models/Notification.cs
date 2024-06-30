namespace University_of_free_state_booking_Facilities.Models
{
    
        public class Notification
        {
            public int Id { get; set; }

        public DateTime DATE { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Username { get; set; }

        public int? BookingId { get; set; }
          
        }
    

}
