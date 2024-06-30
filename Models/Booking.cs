using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace University_of_free_state_booking_Facilities.Models
{

    public class Booking
    {
        public int BookingId { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }
        [Required]
        public string StartTime { get; set; }
        [Required]
        public string EndTime { get; set; }

        [Required]
        public bool IsConfirmed { get; set; }
      
        public decimal TotalAmount { get; set; }

		public string PaymentStatus { get; set; } = string.Empty;
		public string UserName { get; set; }
        public int FacilityId { get; set; }
       

    }


}
