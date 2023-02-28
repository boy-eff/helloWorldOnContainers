using Words.BusinessAccess.Contracts;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.Services;

public class DailyWordCollectionService : IDailyWordCollectionService
{
    public WordCollection DailyWordCollection { get; set; }
}