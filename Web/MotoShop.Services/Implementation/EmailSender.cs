using Microsoft.Extensions.Configuration;
using MotoShop.Services.HelperModels;
using MotoShop.Services.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace MotoShop.Services.Implementation
{
    public class EmailSender : IEmailSenderService
    {
        private readonly IConfiguration _configuration;
        private string API_KEY = Environment.GetEnvironmentVariable("SendGridApiKey");
        private EmailAddress motoShopEmailAdress = new EmailAddress();

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
            _configuration.GetSection("EmailCredentials").Bind(motoShopEmailAdress);
        }
        public async Task<bool> SendConfirmationEmailAsync(EmailAddress recipient, string subject, string verificationLink, EmailType mailType)
        {
            var client = new SendGridClient(API_KEY);

            string contentHtml = string.Empty;
            switch (mailType)
            {
                case EmailType.Verification_PasswordChange:
                    contentHtml = $"To reset your password click <a href='{verificationLink}'>Reset Password</a>";
                    break;
                case EmailType.Verification_Email_Change:
                    contentHtml = $"To reset your email adress click <a href='{verificationLink}'>Change Email</a>";
                    break;
                case EmailType.Verification_Account_Creating:
                    contentHtml = $"Confirm your account by clicking <a href='{verificationLink}'>here</a>";
                    break;
                default:
                    break;
            }

            var msg = MailHelper.CreateSingleEmail(motoShopEmailAdress, recipient, subject, string.Empty, contentHtml);
            var response = await client.SendEmailAsync(msg);

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                return true;

            return false;
        }
        public async Task<bool> SendStandardMessageAsync(EmailAddress recipient, string subject, string content)
        {
            var client = new SendGridClient(API_KEY);

            var msg = MailHelper.CreateSingleEmail(motoShopEmailAdress, recipient, subject, content, string.Empty);

            var result = await client.SendEmailAsync(msg);

            if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
                return true;

            return false;
        }
    }
}
