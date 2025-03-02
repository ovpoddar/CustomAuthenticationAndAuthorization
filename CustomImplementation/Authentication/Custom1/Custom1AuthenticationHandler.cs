using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;

namespace CustomAuthenticationAndAuthorization.CustomImplementation.Authentication.Custom1;

public class Custom1AuthenticationHandler : AuthenticationHandler<Costume1SchemeOptions>
{
    private readonly ClaimsPrincipal _principal;
    public Custom1AuthenticationHandler(IOptionsMonitor<Costume1SchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
    {
        var claim = new ClaimsIdentity(Costume1SchemeOptions.AuthenticationScheme);
        claim.AddClaim(new Claim(ClaimTypes.Actor, "joe", ClaimValueTypes.String, "iso"));
        _principal = new ClaimsPrincipal(claim);
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var isExist = await Task.FromResult(Context.Request.Cookies.TryGetValue("Foo", out var cookie));
        return isExist && !string.IsNullOrWhiteSpace(cookie)
            ? AuthenticateResult.Success(new AuthenticationTicket(_principal, this.Scheme.Name))
            : AuthenticateResult.Fail("no Foo value found.");
    }
}
