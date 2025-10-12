using System.ComponentModel.DataAnnotations;

namespace MVC_Practice_Project.PL.DTOs
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is Required")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Confirm Password Doesn't Match The Password")]
        public string ConfirmNewPassword { get; set; }
    }
}
