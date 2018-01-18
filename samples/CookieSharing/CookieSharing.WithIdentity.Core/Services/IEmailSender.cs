using System.Threading.Tasks;

namespace CookieSharing.WithIdentity.Core.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
