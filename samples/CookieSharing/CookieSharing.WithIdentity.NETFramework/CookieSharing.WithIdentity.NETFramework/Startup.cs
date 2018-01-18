using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CookieSharingWithIdentity.Startup))]
namespace CookieSharingWithIdentity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
