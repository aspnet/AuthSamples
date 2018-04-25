using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace CustomPolicyProvider
{
    // This class contains logic for determining whether MinimumAgeRequirements in authorizaiton
    // policies are satisfied or not.
    internal class MinimumAgeAuthorizationHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        private readonly ILogger<MinimumAgeAuthorizationHandler> _logger;

        public MinimumAgeAuthorizationHandler(ILogger<MinimumAgeAuthorizationHandler> logger)
        {
            _logger = logger;
        }

        // Check whether a given MinimumAgeRequirement is satisfied or not for a particular context
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            // Log as a warning so that it's very clear in sample output which authorization policies 
            // (and requirements/handlers) are in use
            _logger.LogWarning("Evaluating authorization requirement for age >= {age}", requirement.Age);

            // Check the user's age (which won't actually be used in this sample since, in an effort to 
            // keep this sample small, we don't authenticate the user or extract any claims)
            var dateOfBirthClaim = context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth);
            if (dateOfBirthClaim != null)
            {
                // If the user has a date of birth claim, check their age
                var dateOfBirth = Convert.ToDateTime(dateOfBirthClaim);
                var age = DateTime.Now.Year - dateOfBirth.Year;
                if (dateOfBirth > DateTime.Now.AddYears(-age))
                {
                    age--;
                }

                if (age >= requirement.Age)
                {
                    context.Succeed(requirement);
                }
            }

            // For this sample, arbitrarily decide whether to satisfy the requirement since 
            // this simple sample doesn't have any real claims to compare against. This line
            // would, of course, not be used in a real-world application (which would make
            // decisions based on authenticated user claims, instead).
            if (requirement.Age % 2 == 0) context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}