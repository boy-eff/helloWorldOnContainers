using Achievements.Domain;
using Achievements.WebAPI.Extensions;
using Serilog;
using Shared.Extensions;
using Shared.Middleware;
using Words.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.ConfigureLogger();
builder.Services.AddDbContext<AchievementsDbContext>();
builder.Services.AddScopedServices();
builder.Services.AddControllers();
builder.Services.ConfigureSwagger(builder.Configuration);
builder.Services.ConfigureMassTransit(config);
builder.Services.ConfigureAuthentication(config);

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();

app.UseSerilogRequestLogging(x => x.Logger = app.Services.GetService<Serilog.ILogger>());

AppContext.SetSwitch(config["Switches:NpgsqlLegacyTimestampBehavior"], true);

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.ApplyMigrations();


app.Run();