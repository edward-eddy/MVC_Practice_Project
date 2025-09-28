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
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        // ASK CLR Create Object From AppDbContext
        public DepartmentRepository(AppDbContext context) : base(context)
        {

        }
    }
}
