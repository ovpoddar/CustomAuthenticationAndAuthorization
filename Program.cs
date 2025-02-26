using CustomAuthenticationAndAuthorization.CustomImplementation;
using CustomAuthenticationAndAuthorization.CustomImplementation.Authentication.Custom;
using CustomAuthenticationAndAuthorization.CustomImplementation.Authentication.Custom1;
using CustomAuthenticationAndAuthorization.CustomImplementation.Authentication.Forward;
using CustomAuthenticationAndAuthorization.CustomImplementation.Authorization;
using CustomAuthenticationAndAuthorization.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

var forward = new ForwardSchemeHandler();
builder.Services.AddAuthentication(a =>
{
    // add default if its use by every controller
    // [Authorize(AuthenticationSchemes = Costume1SchemeOptions.AuthenticationScheme)] this does not override the default

    a.DefaultScheme = ForwardSchemeHandler.AuthenticationScheme;
    //a.DefaultChallengeScheme = ForwardSchemeHandler.AuthenticationScheme;
})
.AddScheme<CostumeSchemeOptions, CustomAuthenticationHandler>(CostumeSchemeOptions.AuthenticationScheme,
    CostumeSchemeOptions.AuthenticationScheme,
    o => { })
.AddScheme<Costume1SchemeOptions, CustomAuthenticationHandler1>(Costume1SchemeOptions.AuthenticationScheme,
    Costume1SchemeOptions.AuthenticationScheme,
    o => { })
.AddPolicyScheme(ForwardSchemeHandler.AuthenticationScheme, ForwardSchemeHandler.AuthenticationScheme, p =>
{
    // auto selector
    p.ForwardDefaultSelector = forward.ForwardDefaultSelector;
});

builder.Services.AddAuthorization(p =>
{
    p.AddPolicy(nameof(Custom1AuthorizationPolicy), new AuthorizationPolicyBuilder(CostumeSchemeOptions.AuthenticationScheme)
        .AddRequirements(new Custom1AuthorizationPolicy()).Build());

    p.AddPolicy(nameof(CustomAuthorizationPolicy), new AuthorizationPolicyBuilder(Costume1SchemeOptions.AuthenticationScheme)
        .AddRequirements(new CustomAuthorizationPolicy()).Build());

    //var customPolicy = new AuthorizationPolicyBuilder(CostumeSchemeOptions.AuthenticationScheme, Costume1SchemeOptions.AuthenticationScheme)
    //    .AddRequirements(new Custom1AuthorizationPolicy());
    //p.DefaultPolicy = customPolicy.Build();
});

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/weatherforecast", [Authorize(AuthenticationSchemes = Costume1SchemeOptions.AuthenticationScheme)]() =>
{
    var summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

