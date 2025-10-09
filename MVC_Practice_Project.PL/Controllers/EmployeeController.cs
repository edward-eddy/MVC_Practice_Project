using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_Practice_Project.BLL.Interfaces;
using MVC_Practice_Project.DAL.Models;
using MVC_Practice_Project.PL.DTOs;
using MVC_Practice_Project.PL.Helpers;

namespace MVC_Practice_Project.PL.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<Employee>? Employees;

            if (string.IsNullOrEmpty(SearchInput))
            {
                Employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            else
            {
                Employees = await _unitOfWork.EmployeeRepository.GetByNameAsync(SearchInput);
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
        public async Task<IActionResult> Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                if (model.Image is not null)
                {
                    model.ImageName = DocumentSettings.Upload(model.Image, "images");
                }

                var employee = _mapper.Map<Employee>(model);
                await _unitOfWork.EmployeeRepository.AddAsync(employee);
                var Count = await _unitOfWork.CompleteAsync();

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
        public async Task<IActionResult> Details([FromRoute] int? id, string ViewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            if (employee is null) return NotFound(new { statusCode = 404, ErrorMessage = $"Employee with Id: {id} not Found" });


            var employeeDto = _mapper.Map<CreateEmployeeDto>(employee);
            ViewBag.Id = id.Value;
            return View(ViewName, employeeDto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, CreateEmployeeDto model)
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
                var Count = await _unitOfWork.CompleteAsync();

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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest("Invalid Id");
            var Employee = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            if (Employee is null) return NotFound("Not Found!");

            _unitOfWork.EmployeeRepository.Delete(Employee);
            int count = await _unitOfWork.CompleteAsync();

            if (count > 0 && Employee.ImageName is not null)
                DocumentSettings.Delete(Employee.ImageName, "images");


            return RedirectToAction(nameof(Index));
        }
    }
}

