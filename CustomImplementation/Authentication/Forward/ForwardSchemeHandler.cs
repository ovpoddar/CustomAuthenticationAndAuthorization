using CustomAuthenticationAndAuthorization.CustomImplementation.Authentication.Custom;
using CustomAuthenticationAndAuthorization.CustomImplementation.Authentication.Custom1;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Net.Http.Headers;

namespace CustomAuthenticationAndAuthorization.CustomImplementation.Authentication.Forward;

public class ForwardSchemeHandler : PolicySchemeOptions
{
    public ForwardSchemeHandler()
    {
        ForwardDefaultSelector = context =>
        {
            if (context.Request.Headers.ContainsKey("Foo")) return "Custom";
            if (context.Request.Cookies.ContainsKey("Foo")) return "Custom1";
            return null;
        };
    }
    public static string AuthenticationScheme { get; set; } = "ForwardDefaultSelector";
}
