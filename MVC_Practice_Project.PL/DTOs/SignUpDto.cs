﻿using System.ComponentModel.DataAnnotations;

namespace MVC_Practice_Project.PL.DTOs
{
    public class SignUpDto
    {
        [Required(ErrorMessage = "User Name is Required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "First Name is Required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is Required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is Required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Confirm Password Doesn't Match The Password")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "You Must Agree To The Terms To Register")]
        public bool IsAgreed { get; set; }
    }
}
