using Words.DataAccess.Models;

namespace Words.BusinessAccess.Contracts;

public interface IDailyWordCollectionService
{
    public WordCollection DailyWordCollection { get; set; }
}