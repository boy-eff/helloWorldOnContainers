using Microsoft.EntityFrameworkCore;

namespace Words.DataAccess.Extensions;

public static class DatabaseIdentityInsertExtensions
{
    public static Task EnableIdentityInsertAsync<T>(this DbContext context) => SetIdentityInsertAsync<T>(context, enable: true);
    public static Task DisableIdentityInsertAsync<T>(this DbContext context) => SetIdentityInsertAsync<T>(context, enable: false);

    private static Task SetIdentityInsertAsync<T>(DbContext context, bool enable)
    {
        var entityType = context.Model.FindEntityType(typeof(T));
        var value = enable ? "ON" : "OFF";
        return context.Database.ExecuteSqlRawAsync(
            $"SET IDENTITY_INSERT {entityType.GetSchema()}.{entityType.GetTableName()} {value}");
    }

    public static async Task SaveChangesWithIdentityInsert<T>(this DbContext context)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        await context.EnableIdentityInsertAsync<T>();
        await context.SaveChangesAsync();
        await context.DisableIdentityInsertAsync<T>();
        await transaction.CommitAsync();
    }
}