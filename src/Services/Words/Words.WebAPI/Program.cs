using Shared.Extensions;
using Words.BusinessAccess.Contracts;
using Words.BusinessAccess.Services;
using Words.DataAccess;
using Words.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger(builder.Configuration);
builder.Services.AddDbContext<WordsDbContext>();
builder.Services.AddScoped<ICollectionService, CollectionService>();
builder.Services.AddTransient(s => s.GetService<HttpContext>().User);
builder.Services.ConfigureAuthentication(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();