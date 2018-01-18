﻿using System.ComponentModel.DataAnnotations;

namespace CookieSharingWithIdentityCore.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
