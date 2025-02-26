using Microsoft.AspNetCore.Authorization;

namespace CustomAuthenticationAndAuthorization.CustomImplementation.Authorization;

public class Custom1AuthorizationPolicy : AuthorizationHandler<Custom1AuthorizationPolicy>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, Custom1AuthorizationPolicy requirement)
    {
        if (context.User.Claims.Count() > 5)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
        context.Fail();
        return Task.CompletedTask;
    }
}