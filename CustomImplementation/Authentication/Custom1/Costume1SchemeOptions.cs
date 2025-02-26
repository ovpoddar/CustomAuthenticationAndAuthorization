using Microsoft.AspNetCore.Authentication;

namespace CustomAuthenticationAndAuthorization.CustomImplementation.Authentication.Custom1;

public class Costume1SchemeOptions : AuthenticationSchemeOptions
{
    public Costume1SchemeOptions()
    {
    }
    public const string AuthenticationScheme = "Custom1";
}
