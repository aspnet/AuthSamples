using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace PolicyProvider
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
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc();
        }
    }
}
