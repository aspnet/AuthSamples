using System.ComponentModel.DataAnnotations;

namespace CookieSharingWithIdentityCore.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
