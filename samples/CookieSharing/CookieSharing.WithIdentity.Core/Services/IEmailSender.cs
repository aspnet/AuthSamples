using System.Threading.Tasks;

namespace CookieSharingWithIdentityCore.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
