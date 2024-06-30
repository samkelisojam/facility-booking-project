using System.ComponentModel.DataAnnotations;

namespace University_of_free_state_booking_Facilities.Models.ViewModels
{




    public class RegisterStaff 
    {

        [Required(ErrorMessage = "There must be a first name in this field.")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "There must be a last name in this field.")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Display(Name = "Role of Staff")]
        public string UserType { get; set; }
        [Required(ErrorMessage = "You must enter a student number/staff or Identity Number in this section.")]
        [MaxLength(13)]
        public string IdentityNumber { get; set; }

    }
    public class RegisterViewModel
    {
      
     
        [Required(ErrorMessage = "You must enter a student number/staff or Identity Number in this section.")]
        [MaxLength(10)]
        public string IdentityNumber { get; set; }

        [Required(ErrorMessage = "There must be a first name in this field.")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
       
        [Required(ErrorMessage = "There must be a last name in this field.")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The email field must be filled out.")]
        [Display(Name = "E-mail")]
     
        public string Email { get; set; }
    
        [Required(ErrorMessage = "The Password field must be filled out.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string UserType { get; set; }
        [Compare("Password", ErrorMessage = "Passwords must match")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string squestion1 { get; set; }
        public string sQuestion2 { get; set; }
        public string sQuestion3 { get; set; }
    }

    public class UserProfileViewModel
    {
       
   
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
  
        public string SecurityQuestion1 { get; set; }
        public string SecurityQuestion2 { get; set; }
        public string SecurityQuestion3 { get; set; }
        public string ConfirmPassword { get; set; }
    }


}
