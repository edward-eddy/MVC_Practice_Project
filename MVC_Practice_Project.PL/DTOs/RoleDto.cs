using System.ComponentModel.DataAnnotations;

namespace MVC_Practice_Project.PL.DTOs
{
    public class RoleDto
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Role Name is Required")]
        public string Name { get; set; }
    }
}
