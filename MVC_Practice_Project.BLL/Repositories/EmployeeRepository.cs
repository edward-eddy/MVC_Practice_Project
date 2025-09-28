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
        public EmployeeRepository(AppDbContext context) : base(context)
        {

        }
    }
}
