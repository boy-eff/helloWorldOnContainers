using Microsoft.Extensions.Options;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Shared.Extensions;
using Shared.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(builder.Configuration["Ocelot:JsonFile"], optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddSwaggerForOcelot(builder.Configuration);
builder.Services.ConfigureCors(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var corsOptions = app.Services.GetRequiredService<IOptions<CorsConfigurationOptions>>();
app.UseCors(corsOptions.Value.PolicyName);

app.UseRouting();

app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = app.Configuration["Ocelot:PathToSwaggerGen"];
});

app.UseWebSockets();
await app.UseOcelot();

app.Run();