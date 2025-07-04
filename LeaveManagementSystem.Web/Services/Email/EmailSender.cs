
using System.Net.Mail;

namespace LeaveManagementSystem.Web.Services.Email
{
    public class EmailSender(IConfiguration _configuration) : IEmailSender
    {

        //inject values that need to be present to send the email
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //provide all the parameter and validation needed to send an email

            var fromAddress = _configuration["EmailSettings:DefaultEmailAddress"]; //section then key
            var smtpServer = _configuration["EmailSettings:Server"]; 
            var smtpPort = Convert.ToInt32(_configuration["EmailSettings:Port"]); 
            var message = new MailMessage
            {
                From = new MailAddress(fromAddress),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            //each email address needs to be added to " message.To.Add"
            message.To.Add(new MailAddress(email));

            //specify a client. and add server and port
            using var smtp = new SmtpClient(smtpServer, smtpPort);
            //send
            await smtp.SendMailAsync(message);
        }
    }
}
