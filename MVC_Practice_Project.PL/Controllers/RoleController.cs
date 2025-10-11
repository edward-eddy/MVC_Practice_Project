using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_Practice_Project.DAL.Models;
using MVC_Practice_Project.PL.DTOs;
using MVC_Practice_Project.PL.Helpers;
using System.Data;

namespace MVC_Practice_Project.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
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

            var RoleDto = new RoleDto()
            {
                Id = role.Id,
                Name = role.Name
            };

            return View(ViewName, RoleDto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleDto model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role is null) return BadRequest(error: "Invalid Operations !");

                role.Id = model.Id;
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
    }
}
