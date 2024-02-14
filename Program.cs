using API.Data;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAppServices(builder.Configuration);

builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var service = scope.ServiceProvider;

try {
    var dataContext = service.GetRequiredService<DataContext>();
    var userManager = service.GetRequiredService<UserManager<AppUser>>(); //<--
    var roleManager = service.GetRequiredService<RoleManager<AppRole>>(); //<--
    await dataContext.Database.MigrateAsync();
    await Seed.SeedUsers(userManager, roleManager);
}

catch (System.Exception e)
{
    var log = service.GetRequiredService<ILogger<Program>>();
    log.LogError(e, "an error occurred during migration !!");
}

app.Run();