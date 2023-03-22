using FluentValidation;
using Shared.Extensions;
using Words.BusinessAccess.Contracts;
using Words.BusinessAccess.Extensions;
using Words.BusinessAccess.ModelValidators;
using Words.BusinessAccess.Services;
using Words.DataAccess;
using Words.DataAccess.Extensions;
using Words.WebAPI.Extensions;
using Words.WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger(builder.Configuration);
builder.Services.AddDbContext<WordsDbContext>();
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.AddValidatorsFromAssemblyContaining<WordCollectionRequestDtoValidator>();
builder.Services.ConfigureMediatR();
builder.ConfigureLogger();
builder.Services.AddSingleton<IViewsCounterService, ViewsCounterService>();
builder.Services.AddSingleton<IDailyWordCollectionService, DailyWordCollectionService>();
builder.Services.AddScoped<IWordCollectionTestGenerator, WordCollectionTestGenerator>();
builder.Services.ConfigureQuartz(builder.Configuration);
builder.Services.ConfigureMassTransit(builder.Configuration);
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});
builder.Services.RegisterMapsterConfiguration();

var app = builder.Build();
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

public partial class Program { }