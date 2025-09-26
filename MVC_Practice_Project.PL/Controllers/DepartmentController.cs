using Microsoft.AspNetCore.Mvc;
using MVC_Practice_Project.BLL.Interfaces;
using MVC_Practice_Project.BLL.Repositories;
using MVC_Practice_Project.DAL.Models;
using MVC_Practice_Project.PL.DTOs;

namespace MVC_Practice_Project.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;

        // ASK CLR To Create Object From DepartmentRepository
        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;

        }
        public IActionResult Index()
        {
            var departments = _departmentRepository.GetAll();

            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt,
                };
                var Count = _departmentRepository.Add(department);
                if (Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Details([FromRoute] int? id)
        {
            if (id is null) return BadRequest("Invalid Id");
            var department = _departmentRepository.Get(id.Value);
            if (department is null) return NotFound(new { statusCode = 404, ErrorMessage = $"Department with Id: {id} not Found" });

            return View(department);
        }
        [HttpGet]
        public IActionResult Edit([FromRoute] int? id)
        {
            if (id is null) return BadRequest("Invalid Id");
            var department = _departmentRepository.Get(id.Value);
            if (department is null) return NotFound(new { statusCode = 404, ErrorMessage = $"Department with Id: {id} not Found" });

            var dto = new CreateDepartmentDto()
            {
                Code = department.Code,
                Name = department.Name,
                CreateAt = department.CreateAt,
            };
            return View(dto);
        }
        [HttpPost]
        public IActionResult Edit([FromRoute] int id, CreateDepartmentDto model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt,
                    Id = id
                };
                var Count = _departmentRepository.Update(department);
                if (Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id is null) return BadRequest("Invalid Id");
            Department department = _departmentRepository.Get(id.Value);
            if (department is null) return NotFound();

            var Count = _departmentRepository.Delete(department);
            if (Count > 0)
            {
                return RedirectToAction(nameof(Index));
            }


            return View("Index");
        }
    }
}
