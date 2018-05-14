using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AuthSamples.DerivedHandlers
{
    public class DerivedCookieEvents : CookieAuthenticationEvents
    {
        public Func<CookieSigningInContext, Task> OnDerivedSignIn { get; set; }
    }

    public class DerivedCookieOptions : CookieAuthenticationOptions
    {
        public string DerivedThingy { get; set; }
    }

    public class DerivedPostConfigure : IPostConfigureOptions<DerivedCookieOptions>
    {
        private readonly IDataProtectionProvider _dp;

        public DerivedPostConfigure(IDataProtectionProvider dataProtection)
        {
            _dp = dataProtection;
        }

        /// <summary>
        /// Invoked to post configure a TOptions instance.
        /// </summary>
        /// <param name="name">The name of the options instance being configured.</param>
        /// <param name="options">The options instance to configure.</param>
        public void PostConfigure(string name, DerivedCookieOptions options)
        {
            options.DataProtectionProvider = options.DataProtectionProvider ?? _dp;

            if (string.IsNullOrEmpty(options.Cookie.Name))
            {
                options.Cookie.Name = CookieAuthenticationDefaults.CookiePrefix + name;
            }
            if (options.TicketDataFormat == null)
            {
                // Note: the purpose for the data protector must remain fixed for interop to work.
                var dataProtector = options.DataProtectionProvider.CreateProtector("Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationMiddleware", name, "v2");
                options.TicketDataFormat = new TicketDataFormat(dataProtector);
            }
            if (options.CookieManager == null)
            {
                options.CookieManager = new ChunkingCookieManager();
            }
            if (!options.LoginPath.HasValue)
            {
                options.LoginPath = CookieAuthenticationDefaults.LoginPath;
            }
            if (!options.LogoutPath.HasValue)
            {
                options.LogoutPath = CookieAuthenticationDefaults.LogoutPath;
            }
            if (!options.AccessDeniedPath.HasValue)
            {
                options.AccessDeniedPath = CookieAuthenticationDefaults.AccessDeniedPath;
            }
        }
    }

    public class DerivedCookieOptionsMonitor : IOptionsMonitor<CookieAuthenticationOptions>
    {
        private readonly IOptionsMonitor<DerivedCookieOptions> _options;

        public DerivedCookieOptionsMonitor(IOptionsMonitor<DerivedCookieOptions> options) => _options = options;

        public CookieAuthenticationOptions CurrentValue => _options.CurrentValue;

        public CookieAuthenticationOptions Get(string name) => _options.Get(name);

        public IDisposable OnChange(Action<CookieAuthenticationOptions, string> listener)
            => _options.OnChange(listener);
    }

    public static class DerivedExtensions {
        public static AuthenticationBuilder AddDerivedCookie(this AuthenticationBuilder builder, string name, Action<DerivedCookieOptions> configure)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<DerivedCookieOptions>, DerivedPostConfigure>());
            builder.Services.Configure<AuthenticationOptions>(o =>
            {
                o.AddScheme(name, b =>
                {
                    b.HandlerType = typeof(DerivedCookieHandler);
                    b.DisplayName = name;
                });
            });
            if (configure != null)
            {
                builder.Services.Configure(name, configure);
            }
            builder.Services.AddTransient<DerivedCookieHandler>();
            return builder;
        }
    }

    public class DerivedCookieHandler : CookieAuthenticationHandler
    {
        public DerivedCookieHandler(IOptionsMonitor<DerivedCookieOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(new DerivedCookieOptionsMonitor(options), logger, encoder, clock)
        {
        }

        protected new DerivedCookieEvents Events { get => (DerivedCookieEvents)base.Events; set => base.Events = value; }

        public override Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
        {
            return base.SignInAsync(user, properties);
        }

        public override Task SignOutAsync(AuthenticationProperties properties)
        {
            return base.SignOutAsync(properties);
        }

        protected override Task<object> CreateEventsAsync()
        {
            return base.CreateEventsAsync();
        }

        protected override Task FinishResponseAsync()
        {
            return base.FinishResponseAsync();
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            return base.HandleAuthenticateAsync();
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            return base.HandleChallengeAsync(properties);
        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            return base.HandleForbiddenAsync(properties);
        }

        protected override Task HandleSignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
        {
            return base.HandleSignInAsync(user, properties);
        }

        protected override Task HandleSignOutAsync(AuthenticationProperties properties)
        {
            return base.HandleSignOutAsync(properties);
        }

        protected override Task InitializeEventsAsync()
        {
            return base.InitializeEventsAsync();
        }

        protected override Task InitializeHandlerAsync()
        {
            return base.InitializeHandlerAsync();
        }

        protected override string ResolveTarget(string scheme)
        {
            // You can disable scheme forwarding entirely by returning null here.
            return base.ResolveTarget(scheme);
        }
    }
}