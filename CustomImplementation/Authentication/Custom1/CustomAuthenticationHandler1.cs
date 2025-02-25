using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;

namespace CustomAuthenticationAndAuthorization.CustomImplementation.Authentication.Custom1;

public class CustomAuthenticationHandler1 : AuthenticationHandler<Costume1SchemeOptions>
{
    private readonly ClaimsPrincipal _principal;
    private readonly string _authenticationScheme;
    public CustomAuthenticationHandler1(IOptionsMonitor<Costume1SchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
    {
        _authenticationScheme = options.CurrentValue.AuthenticationScheme;
        var claim = new ClaimsIdentity(_authenticationScheme);
        claim.AddClaim(new Claim(ClaimTypes.Actor, "joe", ClaimValueTypes.String, "iso"));
        _principal = new ClaimsPrincipal(claim);
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var isExist = await Task.FromResult(Context.Request.Cookies.TryGetValue("Foo", out var cookie));
        return isExist
            ? AuthenticateResult.Success(new AuthenticationTicket(_principal, _authenticationScheme))
            : AuthenticateResult.Fail("no Foo value found.");
    }
}
