using Microsoft.IdentityModel.Logging;
using Words.BusinessAccess.Contracts;
using Words.BusinessAccess.Services;
using Words.DataAccess;
using Words.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<WordsDbContext>();
builder.Services.AddScoped<ICollectionService, CollectionService>();
builder.Services.AddTransient(s => s.GetService<HttpContext>().User);
builder.Services.ConfigureAuthentication(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();