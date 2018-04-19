using System.ComponentModel.DataAnnotations;

namespace CookieSharing.WithIdentity.Core.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
