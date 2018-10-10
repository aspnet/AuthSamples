using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace CookieSharing.WithIdentity.Core.Models.ManageViewModels
{
    public class ExternalLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationScheme> OtherLogins { get; set; }
        public bool ShowRemoveButton { get; set; }
        public string StatusMessage { get; set; }
    }
}
