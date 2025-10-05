using Microsoft.AspNetCore.Mvc;
using MVC_Practice_Project.BLL.Interfaces;
using MVC_Practice_Project.BLL.Repositories;
using MVC_Practice_Project.DAL.Models;
using MVC_Practice_Project.PL.DTOs;
using System.Threading.Tasks;

namespace MVC_Practice_Project.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        // ASK CLR To Create Object From DepartmentRepository
        public DepartmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();

            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                };
                await _unitOfWork.DepartmentRepository.AddAsync(department);
                var Count = await _unitOfWork.CompleteAsync();
                if (Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Details([FromRoute] int? id, string ViewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            var department = await _unitOfWork.DepartmentRepository.GetAsync(id.Value);
            if (department is null) return NotFound(new { statusCode = 404, ErrorMessage = $"Department with Id: {id} not Found" });

            var departmentDto = new CreateDepartmentDto()
            {
                Code = department.Code,
                Name = department.Name,
            };

            ViewBag.Id = id.Value;
            return View(ViewName, departmentDto);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            //if (id is null) return BadRequest("Invalid Id");
            //var department = _departmentRepository.Get(id.Value);
            //if (department is null) return NotFound(new { statusCode = 404, ErrorMessage = $"Department with Id: {id} not Found" });

            //var dto = new CreateDepartmentDto()
            //{
            //    Code = department.Code,
            //    Name = department.Name,
            //    CreateAt = department.CreateAt,
            //};
            return await Details(id, "Edit");
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, Department department)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                //var department = new Department()
                //{
                //    Code = model.Code,
                //    Name = model.Name,
                //    CreateAt = model.CreateAt,
                //    Id = id
                //};
                department.Id = id;
                _unitOfWork.DepartmentRepository.Update(department);
                var Count = await _unitOfWork.CompleteAsync();
                if (Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(department);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest("Invalid Id");
            var department = await _unitOfWork.DepartmentRepository.GetAsync(id.Value);
            if (department is null) return NotFound("Not Found!");

            _unitOfWork.DepartmentRepository.Delete(department);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
