using System.ComponentModel.DataAnnotations;

namespace University_of_free_state_booking_Facilities.Models.ViewModels
{
    public class CardViewModel
    {
        [Required]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Card Number")]
        public string CardNumber { get; set; }

        [Required]
        [Display(Name = "Expiration Date (MM/YY)")]
        public string ExpirationDate { get; set; }

        [Required]
        [Display(Name = "CVV")]
        [StringLength(3, MinimumLength = 3)]
        public string CVV { get; set; }
       
    }
    public class DepositViewModel
    {
        [Required]
        public int DepositReference { get; set; }

      

    }

}
