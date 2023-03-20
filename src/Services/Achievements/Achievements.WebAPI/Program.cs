using Achievements.Domain;
using Achievements.WebAPI.Extensions;
using Shared.Extensions;
using Words.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Services.AddDbContext<AchievementsDbContext>();
builder.Services.AddScopedServices();
builder.Services.AddControllers();
builder.Services.ConfigureSwagger(builder.Configuration);
builder.Services.ConfigureMassTransit(config);
builder.Services.ConfigureAuthentication(config);

var app = builder.Build();

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