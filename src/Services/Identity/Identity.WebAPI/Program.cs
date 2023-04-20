using System.Text.Json.Serialization;
using Identity.Application.Interfaces;
using Identity.Application.Services;
using Identity.Infrastructure.Data;
using Identity.Infrastructure.Extensions;
using Identity.WebAPI.Extensions;
using Identity.WebAPI.Middleware;
using Microsoft.Extensions.Options;
using Serilog;
using Shared.Extensions;
using Shared.Options;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var env = builder.Environment;

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger(config);
builder.Services.ConfigureAuthentication();
builder.Services.ConfigureAuthorization();
builder.Services.ConfigureDatabaseConnection(config);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureIdentityServer(config);
builder.Services.ConfigureCors(config);
builder.Services.ConfigureMassTransit(config);
builder.Services.AddScoped<IDbContext>(x => x.GetService<AuthDbContext>());
builder.ConfigureLogger();

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();

app.UseSerilogRequestLogging(x => x.Logger = app.Services.GetService<Serilog.ILogger>());

var corsOptions = app.Services.GetRequiredService<IOptions<CorsConfigurationOptions>>();
app.UseCors(corsOptions.Value.PolicyName);

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
