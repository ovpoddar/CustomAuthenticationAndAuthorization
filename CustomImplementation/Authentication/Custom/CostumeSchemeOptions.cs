using Microsoft.AspNetCore.Authentication;

namespace CustomAuthenticationAndAuthorization.CustomImplementation.Authentication.Custom;

public class CostumeSchemeOptions : AuthenticationSchemeOptions
{
    public CostumeSchemeOptions()
    {
    }


    public string AuthenticationScheme { get; set; } = "Custom";
    public string ForwardScheme { get; set; } = "Custom1";
}
