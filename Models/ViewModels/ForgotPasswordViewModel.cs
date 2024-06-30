using System.ComponentModel.DataAnnotations;

namespace University_of_free_state_booking_Facilities.Models.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

}
