using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace CustomAuthenticationAndAuthorization.CustomImplementation.Authentication.Custom;

public class CustomAuthenticationHandler : AuthenticationHandler<CostumeSchemeOptions>
{
    private readonly ClaimsPrincipal _principal;

    public CustomAuthenticationHandler(IOptionsMonitor<CostumeSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
    {
        var claim = new ClaimsIdentity(CostumeSchemeOptions.AuthenticationScheme);
        // adding some claims to verify later is a good idea
        // that's way you can verify which handle the request.
        claim.AddClaim(new Claim(ClaimTypes.Actor, "joe", ClaimValueTypes.String, "iso"));
        _principal = new ClaimsPrincipal(claim);
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var token = Context.Request.Headers["Foo"];
        var responses = await Task.FromResult(!string.IsNullOrWhiteSpace(token));
        return responses
            ? AuthenticateResult.Success(new AuthenticationTicket(_principal, this.Scheme.Name))
            : AuthenticateResult.Fail("no Foo value found.");
    }
}
