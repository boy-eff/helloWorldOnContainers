using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;
using Words.BusinessAccess.Models;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.Extensions;

public static class FilteringExtensions
{
    public static IQueryable<WordCollection> ApplyFilters(this IQueryable<WordCollection> source, FilteringParameters filteringParameters)
    {
        return filteringParameters.EnglishLevel is not null ? source.Where(x => x.EnglishLevel == filteringParameters.EnglishLevel) : source;
    }
}