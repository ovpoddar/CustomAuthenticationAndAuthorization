using CustomAuthenticationAndAuthorization.CustomImplementation.Authentication.Custom;
using CustomAuthenticationAndAuthorization.CustomImplementation.Authentication.Custom1;
using CustomAuthenticationAndAuthorization.CustomImplementation.Authentication.Forward;
using CustomAuthenticationAndAuthorization.CustomImplementation.Authorization;
using CustomAuthenticationAndAuthorization.Models;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);
var forwardSchemeHandler = new ForwardSchemeHandler();
builder.Services.AddAuthentication() 
    // setting the default does not get overide
    // so only use default if you have only one 
    // policy
    .AddPolicyScheme(ForwardSchemeHandler.AuthenticationScheme, ForwardSchemeHandler.AuthenticationScheme, f =>
    {
        f.ForwardDefaultSelector = forwardSchemeHandler.ForwardDefaultSelector;
    })
    .AddScheme<CostumeSchemeOptions, CustomAuthenticationHandler>(CostumeSchemeOptions.AuthenticationScheme, CostumeSchemeOptions.AuthenticationScheme, o => { })
    .AddScheme<Costume1SchemeOptions, Custom1AuthenticationHandler>(Costume1SchemeOptions.AuthenticationScheme, Costume1SchemeOptions.AuthenticationScheme, o => { });

// same goes for policy
builder.Services.AddAuthorization(a =>
{
    a.AddPolicy(nameof(CustomAuthorizationPolicy), new AuthorizationPolicyBuilder()
        .AddRequirements(new CustomAuthorizationPolicy())
        .Build());
    a.AddPolicy(nameof(Custom1AuthorizationPolicy), new AuthorizationPolicyBuilder()
        .AddRequirements(new Custom1AuthorizationPolicy())
        .Build());

    a.AddPolicy("Custom1AuthorizationPolicy", new AuthorizationPolicyBuilder(CostumeSchemeOptions.AuthenticationScheme, Costume1SchemeOptions.AuthenticationScheme)
        .AddRequirements(new Custom1AuthorizationPolicy())
        .Build());
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};
app.MapGet("/weatherforecast/1", [Authorize(AuthenticationSchemes = CostumeSchemeOptions.AuthenticationScheme)] () =>
{
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
.WithName("GetWeatherForecast/1");


app.MapGet("/weatherforecast/2", [Authorize(AuthenticationSchemes = Costume1SchemeOptions.AuthenticationScheme)] () =>
{
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
.WithName("GetWeatherForecast/2");

app.MapGet("/weatherforecast/3", [Authorize(AuthenticationSchemes = ForwardSchemeHandler.AuthenticationScheme)] () =>
{
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
.WithName("GetWeatherForecast/3");

app.MapGet("/weatherforecast/1/1", [Authorize(AuthenticationSchemes = CostumeSchemeOptions.AuthenticationScheme, Policy = nameof(CustomAuthorizationPolicy))] () =>
{
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
.WithName("GetWeatherForecast/1/1");


app.MapGet("/weatherforecast/2/2", [Authorize(AuthenticationSchemes = Costume1SchemeOptions.AuthenticationScheme, Policy = nameof(Custom1AuthorizationPolicy))] () =>
{
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
.WithName("GetWeatherForecast/2/2");

app.MapGet("/weatherforecast/3/3", [Authorize("Custom1AuthorizationPolicy")] () =>
{
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
.WithName("GetWeatherForecast/3/3");
app.Run();
