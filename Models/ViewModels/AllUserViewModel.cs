using System.ComponentModel.DataAnnotations;

namespace University_of_free_state_booking_Facilities.Models.ViewModels
{
    public class ProfileViewModel
    {

        [Required(ErrorMessage = "Enter UserName")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Enter Email-Address")]
        [Display(Name = "E-mail")]
        [EmailAddress]
        public string Email { get; set; }
        public string UserId { get; set; }



        [Required(ErrorMessage = "Please enter Last Name")]
        public string LastName { get; set; }
        public string FirstName { get; set; }


    }
    public class PasswordViewModel
    {
        [Required(ErrorMessage = "Enter Email-Address")]
        [Display(Name = "E-mail")]
        [EmailAddress]

        public string Email { get; set; }
        [Required(ErrorMessage = "Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords must match")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
