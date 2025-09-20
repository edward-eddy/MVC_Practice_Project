using Microsoft.AspNetCore.Mvc;
using MVC_Practice_Project.BLL.Repositories;

namespace MVC_Practice_Project.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly DepartmentRepository _departmentRepository;

        // ASK CLR To Create Object From DepartmentRepository
        public DepartmentController(DepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;

        }
        public IActionResult Index()
        {
            var departments = _departmentRepository.GetAll();

            return View();
        }
    }
}
