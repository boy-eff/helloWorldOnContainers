namespace AuthService.WebAPI.Extensions;

public static class SwaggerExtensions
{
    public static void ConfigureSwagger(this IServiceCollection services, ConfigurationManager config)
    {
        services.AddSwaggerGen(options =>
        {
            var xmlFilePath = config["XmlFilePath"];
            options.IncludeXmlComments(xmlFilePath);
        });
    }
}