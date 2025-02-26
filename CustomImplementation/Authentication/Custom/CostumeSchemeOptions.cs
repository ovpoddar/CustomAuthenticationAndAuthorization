using Microsoft.AspNetCore.Authentication;

namespace CustomAuthenticationAndAuthorization.CustomImplementation.Authentication.Custom;

public class CostumeSchemeOptions : AuthenticationSchemeOptions
{
    public CostumeSchemeOptions()
    {
    }


    public static string AuthenticationScheme { get; set; } = "Custom";
}
