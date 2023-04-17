using Words.BusinessAccess.Pagination;

namespace Words.BusinessAccess.Extensions;

public static class PaginationExtensions
{
    public static IQueryable<T> GetPage<T>(this IQueryable<T> source, PaginationParameters paginationParameters)
    {
        return source.Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
            .Take(paginationParameters.PageSize);
    }
}