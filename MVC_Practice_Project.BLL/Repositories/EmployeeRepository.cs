using Microsoft.EntityFrameworkCore;
using MVC_Practice_Project.BLL.Interfaces;
using MVC_Practice_Project.DAL.Data.Contexts;
using MVC_Practice_Project.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Practice_Project.BLL.Repositories
{
    // ASK CLR Create Object From AppDbContext
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public List<Employee>? GetByName(string Name)
        {
            return _context.Employees.Include(E => E.WorkFor).Where(E => E.Name.ToLower().Contains(Name.ToLower())).ToList();
        }
    }
}
