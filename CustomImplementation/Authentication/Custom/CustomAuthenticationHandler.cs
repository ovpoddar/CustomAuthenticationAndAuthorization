using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace CustomAuthenticationAndAuthorization.CustomImplementation.Authentication.Custom;

public class CustomAuthenticationHandler : AuthenticationHandler<CostumeSchemeOptions>
{
    private readonly ClaimsPrincipal _principal;
    private readonly string _authenticationScheme;
    private readonly IOptionsMonitor<CostumeSchemeOptions> _options;

    public CustomAuthenticationHandler(IOptionsMonitor<CostumeSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
    {
        _authenticationScheme = options.CurrentValue.AuthenticationScheme;
        var claim = new ClaimsIdentity(options.CurrentValue.AuthenticationScheme);
        // adding some claims to verify later is a good idea
        // that's way you can verify which handle the request.
        claim.AddClaim(new Claim(ClaimTypes.Actor, "joe", ClaimValueTypes.String, "iso"));
        _principal = new ClaimsPrincipal(claim);
        _options = options;
    }


    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var canAuthenticate = Context.Request.Headers.ContainsKey("Foo");
        if (!canAuthenticate)
            return _options.CurrentValue.ForwardScheme is null
                ? AuthenticateResult.NoResult()
                : await Context.AuthenticateAsync(_options.CurrentValue.ForwardScheme);
        var token = Context.Request.Headers["Foo"];
        var responses = await Task.FromResult(!string.IsNullOrWhiteSpace(token));
        return responses
            ? AuthenticateResult.Success(new AuthenticationTicket(_principal, _authenticationScheme))
            : AuthenticateResult.Fail("no Foo value found.");
    }
}
