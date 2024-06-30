using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace University_of_free_state_booking_Facilities.Models.ViewModels
{
    public class SignInViewModel
    {
        [Required(ErrorMessage = "You must enter a Email in this section.")]
  
        [DisplayName("Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "The Password field must be filled out.")]
        [UIHint("Password")]
        public string Password { get; set; }
      //  public string ReturnUrl { get; set; } = "/";
       // public bool RememberMe { get; set; }
    }
}
