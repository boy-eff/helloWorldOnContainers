using FluentValidation;
using MediatR;
using Words.BusinessAccess.Features.Collections.Queries.Get;
using Words.BusinessAccess.MediatR;
using Words.BusinessAccess.ModelValidators;
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
builder.Services.AddValidatorsFromAssemblyContaining<WordCollectionCreateDtoValidator>();
builder.Services.ConfigureMediatR();
builder.ConfigureLogger();
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

app.ApplyMigrations();

app.Run();