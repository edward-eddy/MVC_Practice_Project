using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Practice_Project.BLL.Interfaces;
using MVC_Practice_Project.BLL.Repositories;
using MVC_Practice_Project.DAL.Models;
using MVC_Practice_Project.PL.DTOs;
using MVC_Practice_Project.PL.Helpers;

namespace MVC_Practice_Project.PL.Controllers
{
    public class UserController : Controller
    {
        //private readonly UnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public UserController(/*UnitOfWork unitOfWork,*/ UserManager<AppUser> userManager)
        {
            //_unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<UsersIndexTableDto>? Users;

            if (string.IsNullOrEmpty(SearchInput))
            {
                Users = _userManager.Users.Select(U => new UsersIndexTableDto()
                {
                    Id = U.Id,
                    Username = U.UserName,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Email = U.Email,
                    Roles = _userManager.GetRolesAsync(U).Result
                });
            }
            else
            {
                Users = _userManager.Users.Select(U => new UsersIndexTableDto()
                {
                    Id = U.Id,
                    Username = U.UserName,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Email = U.Email,
                    Roles = _userManager.GetRolesAsync(U).Result
                }).Where(U => (string.Concat(U.FirstName, " ", U.LastName)).ToLower().Contains(SearchInput.ToLower()));
            }

            ViewData["SearchInput"] = SearchInput;
            return View(Users);
        }

        [HttpGet]
        public async Task<IActionResult> Details([FromRoute] string? id, string ViewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound(new { statusCode = 404, ErrorMessage = $"Employee with Id: {id} not Found" });

            var UserDto = new UsersIndexTableDto()
            {
                Id = user.Id,
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = _userManager.GetRolesAsync(user).Result
            };
            return View(ViewName, UserDto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, UsersIndexTableDto model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user is not null && user.Id != id) return BadRequest("Username Is Already Taken");

                user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null && user.Id != id) return BadRequest("Email Is Already Been Used");

                user = await _userManager.FindByIdAsync(id);
                if (user is null || id != model.Id) return BadRequest(error: "Invalid Operations !");

                user.UserName = model.Username;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            if (id is null) return BadRequest("Invalid Id");
            var User = await _userManager.FindByIdAsync(id);
            if (User is null) return NotFound("Not Found!");

            var result = await _userManager.DeleteAsync(User);

            //int count = await _unitOfWork.CompleteAsync();

            if (!result.Succeeded)
            {
                return BadRequest("Couldn't Delete The User");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
