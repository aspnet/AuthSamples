using System.ComponentModel.DataAnnotations;

namespace CookieSharing.WithIdentity.Core.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
