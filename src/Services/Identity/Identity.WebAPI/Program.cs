using Identity.Application.Interfaces;
using Identity.Application.Services;
using Identity.Infrastructure.Extensions;
using Identity.WebAPI.Extensions;
using Identity.WebAPI.Middleware;
using Serilog;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var env = builder.Environment;

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger(config);
builder.Services.ConfigureAuthentication();
builder.Services.ConfigureAuthorization();
builder.Services.ConfigureDatabaseConnection(config);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureIdentityServer(config);
builder.Services.ConfigureCors(env);
builder.Services.ConfigureMassTransit(config);
builder.ConfigureLogger();

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();

app.UseSerilogRequestLogging(x => x.Logger = app.Services.GetService<Serilog.ILogger>());

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
