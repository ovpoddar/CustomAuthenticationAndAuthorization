using Microsoft.AspNetCore.Authentication;

namespace CustomAuthenticationAndAuthorization.CustomImplementation.Authentication.Custom;

public class CostumeSchemeOptions : AuthenticationSchemeOptions
{
    public CostumeSchemeOptions()
    {
    }

    public const string AuthenticationScheme  = "Custom";
}
