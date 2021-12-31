using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace ShopApp.WebUI.EmailServices
{
    public class EmailSender : IEmailSender
    {
        private const string SendGridKey = "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";
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(SendGridKey, subject, htmlMessage, email);
        }

        private Task Execute(string sendGridKey, string subject, string message, string email)
        {
            var client = new SendGridClient(sendGridKey);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress("tanjubozok@hotmail.com", "Test Proje"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };

            msg.AddTo(new EmailAddress(email));
            return client.SendEmailAsync(msg);
        }
    }
}
