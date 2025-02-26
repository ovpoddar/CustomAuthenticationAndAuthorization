using CustomAuthenticationAndAuthorization.CustomImplementation;
using CustomAuthenticationAndAuthorization.CustomImplementation.Authentication.Custom;
using CustomAuthenticationAndAuthorization.CustomImplementation.Authentication.Custom1;
using CustomAuthenticationAndAuthorization.Models;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

var forward = new ForwardSchemeHandler();
builder.Services.AddAuthentication(a =>
{
    a.DefaultScheme = ForwardSchemeHandler.AuthenticationScheme;
    a.DefaultChallengeScheme = ForwardSchemeHandler.AuthenticationScheme;
})
.AddScheme<CostumeSchemeOptions, CustomAuthenticationHandler>(CostumeSchemeOptions.AuthenticationScheme,
    CostumeSchemeOptions.AuthenticationScheme,
    o => { })
.AddScheme<Costume1SchemeOptions, CustomAuthenticationHandler1>(Costume1SchemeOptions.AuthenticationScheme,
    Costume1SchemeOptions.AuthenticationScheme,
    o => { })
.AddPolicyScheme(ForwardSchemeHandler.AuthenticationScheme, ForwardSchemeHandler.AuthenticationScheme, p =>
{
    p.ForwardDefaultSelector = forward.ForwardDefaultSelector;
});

builder.Services.AddAuthorization(p =>
{
    p.AddPolicy(nameof(Custom1AuthorizationPolicy), new AuthorizationPolicyBuilder()
        .AddRequirements(new Custom1AuthorizationPolicy()).Build());

    p.AddPolicy(nameof(CustomAuthorizationPolicy), new AuthorizationPolicyBuilder()
        .AddRequirements(new CustomAuthorizationPolicy()).Build());
});

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/weatherforecast", () =>
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
.WithName("GetWeatherForecast")
.RequireAuthorization();

app.Run();

