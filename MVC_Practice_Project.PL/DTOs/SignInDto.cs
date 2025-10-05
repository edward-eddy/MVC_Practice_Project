namespace MVC_Practice_Project.PL.DTOs
{
    public class SignInDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsRemembered { get; set; }
    }
}
