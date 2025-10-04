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
        private readonly IUnitOfWork _unitOfWork;

        // ASK CLR To Create Object From DepartmentRepository
        public DepartmentController(/*IDepartmentRepository departmentRepository*/IUnitOfWork unitOfWork)
        {
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();

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
                };
                _unitOfWork.DepartmentRepository.Add(department);
                var Count = _unitOfWork.Complete();
                if (Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Details([FromRoute] int? id, string ViewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            var department = _unitOfWork.DepartmentRepository.Get(id.Value);
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
        public IActionResult Edit(int? id)
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
            return Details(id, "Edit");
        }
        [HttpPost]
        public IActionResult Edit([FromRoute] int id, Department department)
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
                var Count = _unitOfWork.Complete();
                if (Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(department);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id is null) return BadRequest("Invalid Id");
            var department = _unitOfWork.DepartmentRepository.Get(id.Value);
            if (department is null) return NotFound("Not Found!");

            _unitOfWork.DepartmentRepository.Delete(department);

            return RedirectToAction(nameof(Index));
        }
    }
}
