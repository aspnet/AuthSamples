using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AuthSamples.DynamicSchemes.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IOptionsMonitorCache<SimpleOptions> _optionsCache;

        public AuthController(IAuthenticationSchemeProvider schemeProvider, IOptionsMonitorCache<SimpleOptions> optionsCache)
        {
            _schemeProvider = schemeProvider;
            _optionsCache = optionsCache;
        }

        public IActionResult Remove(string scheme)
        {
            _schemeProvider.RemoveScheme(scheme);
            _optionsCache.TryRemove(scheme);
            return Redirect("/");
        }

        [HttpPost]
        public async Task<IActionResult> Add(string scheme, string optionsMessage)
        {
            if (await _schemeProvider.GetSchemeAsync(scheme) != null)
            {
                throw new InvalidOperationException("Scheme already exists.");
            }
            _optionsCache.TryAdd(scheme, new SimpleOptions { DisplayMessage = optionsMessage });
            _schemeProvider.AddScheme(new AuthenticationScheme(scheme, scheme, typeof(SimpleAuthHandler)));
            return Redirect("/");
        }
    }
}
