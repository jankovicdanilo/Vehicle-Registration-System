using MailKit.Security;
using MimeKit;
using VehicleRegistrationSystem.Services.Interface;
using MailKit.Net.Smtp;
using VehicleRegistrationSystem.Models.DTO;
using Microsoft.Extensions.Options;

namespace VehicleRegistrationSystem.Services.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            this.emailSettings = emailSettings.Value;
        }

        public async Task SendConfirmationEmailAsync(string toEmail, byte[] pdfBytes)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("jankovic.danilo23@gmail.com"));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = "Vehicle Registration Confirmation";

            var builder = new BodyBuilder
            {
                TextBody = 
                "Dear Sir/Madam,\n\nAttached you will find the confirmation of your vehicle registration." +
                "\n\nKind regards,\nMinistry of Interior of the Republic of Serbia"
            };

            builder.Attachments.Add
                ("RegistrationConfirmation.pdf", pdfBytes, new ContentType("application", "pdf"));
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(emailSettings.SmtpServer, emailSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(emailSettings.Email, emailSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}