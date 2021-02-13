using MotoShop.Services.HelperModels;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace MotoShop.Services.Services
{
    public interface IEmailSenderService
    {
        Task<bool> SendConfirmationEmailAsync(EmailAddress recipient, string subject, string verificationLink, EmailType mailType);
        Task<bool> SendStandardMessageAsync(EmailAddress recipient, string subject, string content);
    }
}
