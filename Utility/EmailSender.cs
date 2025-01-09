using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Utility
{
    public class EmailSender:IEmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }


        public Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpHost = _config["EmailSettings:SMTPHost"];
            var smtpPort = int.Parse(_config["EmailSettings:SMTPPort"]);
            var senderEmail = _config["EmailSettings:SenderEmail"];
            var senderPassword = _config["EmailSettings:SenderPassword"];
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail, senderPassword)
            };

            return client.SendMailAsync(
                new MailMessage(from: senderEmail,
                                to: email,
                                subject,
                                message
                                ));
        }
    }

   
    
}
    