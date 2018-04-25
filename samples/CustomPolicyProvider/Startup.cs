using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace CustomPolicyProvider
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // An IAuthorizationPolicyProvider can take the place of the usual AddAuthorization call
            // since it will dynamically produce necessary authorization policies (as opposed to having 
            // them added via AddPolicy calls on an AuthorizationOptions object).
            services.AddSingleton<IAuthorizationPolicyProvider, MinimumAgePolicyProvider>();

            // As always, handlers must be provided for the requirements of the authorization policies
            services.AddSingleton<IAuthorizationHandler, MinimumAgeAuthorizationHandler>();

            services.AddMvc();

            // Add cookie authentication so that it's possible to sign-in to test the 
            // custom authorization policy behavior of the sample
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.AccessDeniedPath = "/account/denied";
                    options.LoginPath = "/account/signin";
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
            });
        }
    }
}
