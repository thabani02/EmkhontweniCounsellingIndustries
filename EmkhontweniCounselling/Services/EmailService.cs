using System.Net;
using System.Net.Mail;

namespace EmkhontweniCounselling.Services
{
    public class EmailService
    {
        // INSTANCE method (NOT static)
        public void Send(string to, string subject, string body)
        {
            var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(
                    "your@email.com",
                    "your-app-password"
                ),
                EnableSsl = true
            };

            var message = new MailMessage
            {
                From = new MailAddress(
                    "your@email.com",
                    "Emkhontweni Royal Industries"
                ),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(to);
            smtp.Send(message);
        }
    }
}
