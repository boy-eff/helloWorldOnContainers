using System.Reflection;
using IdentityServer4.AccessTokenValidation;
using Microsoft.OpenApi.Models;

namespace Words.WebAPI.Extensions;

public static class SwaggerExtensions
{
    public static void ConfigureSwagger(this IServiceCollection services, ConfigurationManager config)
    {
        services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
            
            options.AddSecurityDefinition(name: IdentityServerAuthenticationDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = IdentityServerAuthenticationDefaults.AuthenticationScheme
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Name = IdentityServerAuthenticationDefaults.AuthenticationScheme,
                        In = ParameterLocation.Header,
                        Reference = new OpenApiReference
                        {
                            Id = IdentityServerAuthenticationDefaults.AuthenticationScheme,
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string>()
                }
            });
        });
    }
}