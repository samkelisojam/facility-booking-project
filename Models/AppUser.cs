using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace University_of_free_state_booking_Facilities.Models
{
    public class AppUser : IdentityUser
    {
        public string IdentityNumber { get; set; }
     
   
        [Required(ErrorMessage = "Please enter name")]
        [DisplayName("First name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter name")]
        [DisplayName("First name")]

        public string LastName { get; set; }
        [Required(ErrorMessage = "Please enter Last Name")]
        [DisplayName("Last name")]
        public string userType { get; set; }
      
        public string squestion1 { get; set; }
        public string sQuestion2 { get; set; }
        public string sQuestion3 { get; set; }
      
        
        [Compare("Password", ErrorMessage = "Passwords must match")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string facility { get; set; } = string.Empty;

    }
}
