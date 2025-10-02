using MVC_Practice_Project.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Practice_Project.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        List<Employee>? GetByName(string Name);
    }
}
