using System.ComponentModel.DataAnnotations;

namespace MVC_Practice_Project.PL.Helpers
{
    public class Email
    {
        [Required]
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
