using CustomAuthenticationAndAuthorization.CustomImplementation;
using CustomAuthenticationAndAuthorization.CustomImplementation.Authentication.Custom;
using CustomAuthenticationAndAuthorization.CustomImplementation.Authentication.Custom1;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = "Custom";
})
.AddScheme<CostumeSchemeOptions, CustomAuthenticationHandler>("Custom", o => { })
.AddScheme<Costume1SchemeOptions, CustomAuthenticationHandler1>("Custom1", o => { });
builder.Services.AddAuthorization(p =>
{
    var customPolicy = new AuthorizationPolicyBuilder()
        .AddRequirements(new CustomAuthorizationPolicy());
    p.DefaultPolicy = customPolicy.Build();
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

