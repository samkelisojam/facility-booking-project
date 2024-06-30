namespace University_of_free_state_booking_Facilities.Models
{
    using System.Collections.Generic;


    public class Facility
    {
        public int FacilityId { get; set; } 

        public string Name { get; set; } 
        public string Description { get; set; }
        public string ImageThumbnailUrl { get; set; }= string.Empty;
        public decimal Price { get; set; }

   
        public int MaxUsersAtOnce { get; set; }
        public string InCharge { get; set; } = string.Empty;
        public List<Booking> Bookings { get; set; }
       
     
        
    }


}
