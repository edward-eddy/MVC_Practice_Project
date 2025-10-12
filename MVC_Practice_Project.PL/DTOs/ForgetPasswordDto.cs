using System.ComponentModel.DataAnnotations;

namespace MVC_Practice_Project.PL.DTOs
{
    public class ForgetPasswordDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
