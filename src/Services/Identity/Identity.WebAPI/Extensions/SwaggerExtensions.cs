using System.Reflection;

namespace Identity.WebAPI.Extensions;

public static class SwaggerExtensions
{
    public static void ConfigureSwagger(this IServiceCollection services, ConfigurationManager config)
    {
        services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });
    }
}