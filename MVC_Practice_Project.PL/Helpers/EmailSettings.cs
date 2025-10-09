using Microsoft.AspNetCore.Rewrite;
using System.Net;
using System.Net.Mail;

namespace MVC_Practice_Project.PL.Helpers
{
    public static class EmailSettings
    {
        public static bool SendEmail(Email email)
        {
            // Mail Server : Gmail
            // SMTP

            try
            {
                var client = new SmtpClient(host: "smtp.gmail.com", port: 587);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("edwardsameh99@gmail.com", "soykrpkxjymkzsqy"); // Sender
                client.Send("edwardsameh99@gmail.com", email.To, email.Subject, email.Body);

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }
    }
}
