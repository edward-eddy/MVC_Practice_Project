using System.ComponentModel.DataAnnotations;

namespace MVC_Practice_Project.PL.DTOs
{
    public class CreateRoleDto
    {
        [Required(ErrorMessage = "Role Name is Required")]
        public string Name { get; set; }
    }
}
