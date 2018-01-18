using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CookieSharing.WithIdentity.NETFramework.Startup))]
namespace CookieSharing.WithIdentity.NETFramework
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
