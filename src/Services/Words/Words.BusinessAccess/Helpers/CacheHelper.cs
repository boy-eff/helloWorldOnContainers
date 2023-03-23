using Microsoft.Extensions.Caching.Distributed;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.Helpers;

public static class CacheHelper
{
    public static string GetCacheKeyForWordCollection(int id) => nameof(WordCollection) + id;
}