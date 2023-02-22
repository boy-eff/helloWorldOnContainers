using Identity.Application.Interfaces;
using Identity.Application.Services;
using Identity.Infrastructure.Extensions;
using Identity.WebAPI.Extensions;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var env = builder.Environment;

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger(config);
builder.Services.ConfigureAuthentication();
builder.Services.AddAuthorization();
builder.Services.ConfigureDatabaseConnection(config);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureIdentityServer();
builder.Services.ConfigureCors(env);

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseIdentityServer();
app.MapControllers();

app.ApplyMigrations();

app.Run();
