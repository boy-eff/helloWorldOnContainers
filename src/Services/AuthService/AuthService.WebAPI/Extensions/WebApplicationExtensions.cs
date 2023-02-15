using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthService.WebAPI.Extensions;

public static class WebApplicationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var services = app.ApplicationServices.CreateScope();

        var dbContext = services.ServiceProvider.GetService<AuthDbContext>();
        if (dbContext is not null)
        { 
            dbContext.Database.Migrate();
        }
    }
}