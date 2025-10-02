using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Practice_Project.DAL.Models
{
    public class Department : BaseEntity
    {
        //public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public List<Employee>? Employees { get; set; }
    }
}
