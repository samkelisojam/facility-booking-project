using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using University_of_free_state_booking_Facilities.Models.ViewModels;
using System.Text;
using System.Net;
using System.Net.Mail;

using System.Diagnostics;
using University_of_free_state_booking_Facilities.Models;
using University_of_free_state_booking_Facilities.Data;

namespace University_of_free_state_booking_Facilities.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppIdentityDbContext _context;
        private readonly IUserValidator<AppUser> _userValidator;
        public AccountController(UserManager<AppUser> userManager,
                                 SignInManager<AppUser> signInManager,
                                 RoleManager<IdentityRole> roleManager, AppIdentityDbContext context,IUserValidator<AppUser> userValidator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _userValidator = userValidator;
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            
            if (ModelState.IsValid)
            {



                AppUser user = new()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    IdentityNumber = model.IdentityNumber,
                    userType = model.UserType,

                    ConfirmPassword = model.ConfirmPassword,
                    UserName = model.FirstName,
                  
                    squestion1 = model.squestion1,
                    sQuestion2 = model.sQuestion2,
                    sQuestion3 = model.sQuestion3

                };




                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (await _roleManager.FindByIdAsync(model.UserType) == null)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(model.UserType));
                    }
                    // Assign user role based on UserType
                    if (!string.IsNullOrEmpty(model.UserType))
                    {
                        await _userManager.AddToRoleAsync(user, model.UserType);
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

    
        [AllowAnonymous]
        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost]

        public async Task<IActionResult> LogIn(SignInViewModel loginModel)
        {

            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByEmailAsync(loginModel.Email);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();
                    var result = await _signInManager.PasswordSignInAsync(user,
                            loginModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ModelState.AddModelError("", "Invalid username or password");
            return View(loginModel);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {


                    TempData["userFound"] = "Any Email has been send with Once off Password you requestd to change it after log In";
					return RedirectToAction("LogIn", "Account");
				}
                else
                {
                    ModelState.AddModelError(string.Empty, "User with the provided email not found.");
                }
            }

            return View(model);
        }





        [AllowAnonymous]
      
        public async Task<ActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            PasswordViewModel passaword = new PasswordViewModel();
            passaword.Email = user.Email;


            return View(passaword);
        }
        [AllowAnonymous]
        [HttpPost]

        public async Task<ActionResult> ChangePassword(PasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(model.Password))
                {
                    if (await _userManager.HasPasswordAsync(user))
                    {
                        await _userManager.RemovePasswordAsync(user);
                    }

                    validPass = await _userManager.AddPasswordAsync(user, model.Password);

                    if (!validPass.Succeeded)
                    {
                        AddErrorsFromResult(validPass);
                    }
                }

                if ((validPass == null)
                      || (model.Password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await _userManager.UpdateAsync(user);

                  

                    if (result.Succeeded)
                    {
                        string successMessage = $"Your password has been successfully change  {user.UserName} LogIn !!.";

                        // Store the message in TempData
                        TempData["ProfileEditedMessage"] = successMessage;

                        return RedirectToAction("LogIn", "Account");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }

            }



            return View(model);
        }

       

     

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            ProfileViewModel person = new ProfileViewModel();
            person.UserName = user.UserName;
            person.FirstName =user.FirstName;
            person.LastName = user.LastName;
            person.UserId = user.Id;
            person.Email = user.Email;
            return View(person);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    user.Email = model.Email;
                    IdentityResult validEmail
                        = await _userValidator.ValidateAsync(_userManager, user);

                    if (!validEmail.Succeeded)
                    {
                        AddErrorsFromResult(validEmail);
                    }
               
                    user.UserName = model.UserName;

                    user.UserName = model.UserName;
                    IdentityResult validName = await _userValidator.ValidateAsync(_userManager, user);

                    if (!validName.Succeeded)
                    {
                        AddErrorsFromResult(validName);
                    }

                    if (
                      validEmail.Succeeded)
                    {
                        IdentityResult result = await _userManager.UpdateAsync(user);


                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            AddErrorsFromResult(result);
                            return View(model);
                        }

                    }
                    return View(model);

                }
            
            }
            return View(model);
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }

}







