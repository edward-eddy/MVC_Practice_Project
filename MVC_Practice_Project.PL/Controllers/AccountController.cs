using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_Practice_Project.DAL.Models;
using MVC_Practice_Project.PL.DTOs;
using MVC_Practice_Project.PL.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MVC_Practice_Project.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        #region SignUp

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user is null)
                {
                    user = await _userManager.FindByEmailAsync(model.Email);
                    if (user is null)
                    {
                        user = new AppUser()
                        {
                            UserName = model.UserName,
                            Email = model.Email,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            IsAgree = model.IsAgreed
                        };

                        var result = await _userManager.CreateAsync(user, model.Password);

                        if (result.Succeeded)
                        {
                            return RedirectToAction("SignIn");
                        }

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }

                ModelState.AddModelError("", "Invalid SignUp");

            }
            return View(model);
        }

        #endregion

        #region SignIn

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (flag)
                    {
                        // Sign In
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.IsRemembered, false);

                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                    }
                }
                ModelState.AddModelError("", "Invalid SignUp");
            }
            return View(model);
        }

        #endregion

        #region SignOut
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn), "Account");
        }
        #endregion

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendResetPasswordURL(ForgetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    // Generate Token
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    // Create URL
                    var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token }, Request.Scheme);


                    // Create Email
                    var email = new Email()
                    {
                        To = model.Email,
                        Subject = "Reset PAssword",
                        Body = url
                    };

                    // Send Email
                    var flag = EmailSettings.SendEmail(email);
                    if (flag)
                    {
                        return RedirectToAction("CheckYourInbox");
                    }
                }
            }
            ModelState.AddModelError("", "Invalid Reset Operation");
            return View("ForgetPassword", model);
        }

        [HttpGet]
        public IActionResult CheckYourInbox()
        {
            return View();
        }
    }
}
