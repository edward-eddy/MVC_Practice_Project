using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_Practice_Project.DAL.Models;
using MVC_Practice_Project.PL.DTOs;
using System.Threading.Tasks;

namespace MVC_Practice_Project.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
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

                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                }
                ModelState.AddModelError("", "Invalid SignUp");
            }
            return View(model);
        }

        #endregion

        #region SignOut

        #endregion



    }
}
