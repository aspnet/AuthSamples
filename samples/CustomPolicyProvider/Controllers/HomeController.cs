using Microsoft.AspNetCore.Mvc;

namespace CustomPolicyProvider.Controllers
{
    // Sample actions to demonstrate the use of the [MinimumAgeAuthorize] attribute
    [Controller]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // View protected with custom parameterized authorization policy
        [MinimumAgeAuthorize(10)]
        public IActionResult MinimumAge10()
        {
            return View();
        }

        // View protected with custom parameterized authorization policy
        [MinimumAgeAuthorize(30)]
        public IActionResult MinimumAge30()
        {
            return View();
        }
    }
}
