using Microsoft.AspNetCore.Authentication;

namespace CustomAuthenticationAndAuthorization.CustomImplementation.Authentication.Custom1;

public class Costume1SchemeOptions : AuthenticationSchemeOptions
{
    public Costume1SchemeOptions()
    {
    }
    public string? ForwardTo { get; set; }
    public string AuthenticationScheme { get; set; } = "Custom1";
}
