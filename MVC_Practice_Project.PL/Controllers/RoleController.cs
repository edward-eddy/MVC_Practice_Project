using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Practice_Project.DAL.Models;
using MVC_Practice_Project.PL.DTOs;
using MVC_Practice_Project.PL.Helpers;
using System.Data;

namespace MVC_Practice_Project.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<RoleDto>? Roles;

            if (string.IsNullOrEmpty(SearchInput))
            {
                Roles = _roleManager.Roles.Select(U => new RoleDto()
                {
                    Id = U.Id,
                    Name = U.Name
                });
            }
            else
            {
                Roles = _roleManager.Roles.Select(U => new RoleDto()
                {
                    Id = U.Id,
                    Name = U.Name
                }).Where(U => (U.Name.ToLower().Contains(SearchInput.ToLower())));
            }

            ViewData["SearchInput"] = SearchInput;
            return View(Roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleDto model)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole()
                {
                    Name = model.Name
                };
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    TempData["Popup"] = "Role Created Successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details([FromRoute] string? id, string ViewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null) return NotFound(new { statusCode = 404, ErrorMessage = $"Role with Id: {id} not Found" });

            var RoleDto = new CreateRoleDto()
            {
                Name = role.Name
            };
            ViewBag.RoleId = id;

            return View(ViewName, RoleDto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, CreateRoleDto model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role is null) return BadRequest(error: "Invalid Operations !");

                role.Id = id;
                role.Name = model.Name;

                var result = await _roleManager.UpdateAsync(role);

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
            var User = await _roleManager.FindByIdAsync(id);
            if (User is null) return NotFound("Not Found!");

            var result = await _roleManager.DeleteAsync(User);

            //int count = await _unitOfWork.CompleteAsync();

            if (!result.Succeeded)
            {
                return BadRequest("Couldn't Delete The User");
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) return NotFound();
            var AllUsersInRole = new List<UserInRoleDto>();
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userInRole = new UserInRoleDto()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    IsSelected = await _userManager.IsInRoleAsync(user, role.Name)
                };

                AllUsersInRole.Add(userInRole);
            }
            ViewData["RoleName"] = role.Name;
            ViewData["RoleId"] = role.Id;
            return View(AllUsersInRole);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId, List<UserInRoleDto> roleUsers)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) return NotFound();
            if (ModelState.IsValid)
            {
                foreach (var user in roleUsers)
                {
                    var appUser = await _userManager.FindByIdAsync(user.UserId);
                    if (appUser is not null)
                    {
                        if (user.IsSelected && !await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.AddToRoleAsync(appUser, role.Name);
                        }
                        else if (!user.IsSelected && await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(appUser, role.Name);
                        }
                    }
                }
                return RedirectToAction(nameof(Edit), new { id = role.Id });
            }
            return View(roleUsers);
        }
    }
}
