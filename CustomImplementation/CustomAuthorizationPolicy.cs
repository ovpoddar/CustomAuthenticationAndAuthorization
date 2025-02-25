using System;
using Microsoft.AspNetCore.Authorization;

namespace CustomAuthenticationAndAuthorization.CustomImplementation;


public class CustomAuthorizationPolicy : AuthorizationHandler<CustomAuthorizationPolicy>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomAuthorizationPolicy requirement)
    {
        if (context.User.Claims.Any())
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
        context.Fail();
        return Task.CompletedTask;
    }
}
