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

            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Password = new OpenApiOAuthFlow()
                    {
                        TokenUrl = new Uri("https://localhost:8001/connect/token")
                    }
                }
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