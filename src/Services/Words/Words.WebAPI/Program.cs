using FluentValidation;
using Words.BusinessAccess.Features.Collections.Queries;
using Words.BusinessAccess.Validators;
using Words.DataAccess;
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
builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(GetWordCollectionsQuery).Assembly));
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

app.Run();