using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.Extensions.Options;
using Serilog;
using Shared.Extensions;
using Shared.Middleware;
using Shared.Options;
using Words.BusinessAccess.Contracts;
using Words.BusinessAccess.Extensions;
using Words.BusinessAccess.ModelValidators;
using Words.BusinessAccess.Services;
using Words.DataAccess;
using Words.DataAccess.Extensions;
using Words.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.ConfigureSwagger(builder.Configuration);
builder.Services.AddDbContext<WordsDbContext>();
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureAuthorization();
builder.Services.AddValidatorsFromAssemblyContaining<WordCollectionRequestDtoValidator>();
builder.Services.ConfigureMediatR();
builder.ConfigureLogger();
builder.Services.AddSingleton<IViewsCounterService, ViewsCounterService>();
builder.Services.AddSingleton<IDailyWordCollectionService, DailyWordCollectionService>();
builder.Services.AddScoped<IWordCollectionTestGenerator, WordCollectionTestGenerator>();
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
builder.Services.ConfigureQuartz(builder.Configuration);
builder.Services.ConfigureMassTransit(builder.Configuration);
builder.Services.AddSignalR();
builder.Services.RegisterMapsterConfiguration();
builder.Services.ConfigureRedis(builder.Configuration);
builder.Services.ConfigureCloudinaryOptions(builder.Configuration);
builder.Services.ConfigureCors(builder.Configuration);

var app = builder.Build();

var corsOptions = app.Services.GetRequiredService<IOptions<CorsConfigurationOptions>>();
app.UseCors(corsOptions.Value.PolicyName);

app.UseSerilogRequestLogging(x => x.Logger = app.Services.GetService<Serilog.ILogger>());

app.UseMiddleware<ExceptionMiddleware>();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseSignalR();

app.ApplyMigrations();

app.Run();