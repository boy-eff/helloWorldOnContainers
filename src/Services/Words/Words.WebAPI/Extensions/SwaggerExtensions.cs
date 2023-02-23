using System.Reflection;
using IdentityServer4.AccessTokenValidation;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Words.WebAPI.Swagger;

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
            
            options.OperationFilter<AuthorizeCheckOperationFilter>();
            
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Password = new OpenApiOAuthFlow()
                    {
                        TokenUrl = new Uri(config["IdentityServer:Token"]),
                    }
                }
            });
            
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = IdentityServerAuthenticationDefaults.AuthenticationScheme
                        },
                        Scheme = IdentityServerAuthenticationDefaults.AuthenticationScheme,
                        In = ParameterLocation.Header
                    },
                    new string[] {}
                }
            }); 
        }); 
    }
}