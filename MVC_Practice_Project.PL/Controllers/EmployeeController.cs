using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MVC_Practice_Project.BLL.Interfaces;
using MVC_Practice_Project.BLL.Repositories;
using MVC_Practice_Project.DAL.Models;
using MVC_Practice_Project.PL.DTOs;
using MVC_Practice_Project.PL.Helpers;

namespace MVC_Practice_Project.PL.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeeController(
                                IUnitOfWork unitOfWork,
                                IMapper mapper
                                )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<Employee>? Employees;

            if (string.IsNullOrEmpty(SearchInput))
            {
                Employees = _unitOfWork.EmployeeRepository.GetAll();
            }
            else
            {
                Employees = _unitOfWork.EmployeeRepository.GetByName(SearchInput);
            }

            ViewData["SearchInput"] = SearchInput;
            return View(Employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                if (model.Image is not null)
                {
                    model.ImageName = DocumentSettings.Upload(model.Image, "images");
                }

                var employee = _mapper.Map<Employee>(model);
                _unitOfWork.EmployeeRepository.Add(employee);
                var Count = _unitOfWork.Complete();

                if (Count > 0)
                {
                    TempData["Popup"] = "Employee Added Successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    DocumentSettings.Delete(model.ImageName, "images");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Details([FromRoute] int? id, string ViewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            var employee = _unitOfWork.EmployeeRepository.Get(id.Value);
            if (employee is null) return NotFound(new { statusCode = 404, ErrorMessage = $"Employee with Id: {id} not Found" });


            var employeeDto = _mapper.Map<CreateEmployeeDto>(employee);
            ViewBag.Id = id.Value;
            return View(ViewName, employeeDto);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            //var departments = _departmentRepository.GetAll();
            //ViewBag.Departments = departments;
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, CreateEmployeeDto model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                if (model.ImageName is not null && model.Image is not null)
                {
                    DocumentSettings.Delete(model.ImageName, "images");
                    model.ImageName = DocumentSettings.Upload(model.Image, "images");

                }
                else if (model.Image is not null)
                {
                    model.ImageName = DocumentSettings.Upload(model.Image, "images");
                }

                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;

                _unitOfWork.EmployeeRepository.Update(employee);
                var Count = _unitOfWork.Complete();

                if (Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    if (model.Image is not null)
                    {
                        DocumentSettings.Delete(model.ImageName, "images");
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id is null) return BadRequest("Invalid Id");
            var department = _unitOfWork.EmployeeRepository.Get(id.Value);
            if (department is null) return NotFound("Not Found!");

            _unitOfWork.EmployeeRepository.Delete(department);
            int count = _unitOfWork.Complete();

            if (count > 0 && department.ImageName is not null)
                DocumentSettings.Delete(department.ImageName, "images");


            return RedirectToAction(nameof(Index));
        }
    }
}
}
