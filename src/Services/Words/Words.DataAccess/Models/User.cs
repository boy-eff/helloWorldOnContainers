using Shared.Enums;

namespace Words.DataAccess.Models;

public class User
{
    public int Id { get; set; }
    public EnglishLevel EnglishLevel { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

    public List<WordCollection> Collections { get; set; }
    public List<WordCollectionRate> CollectionRates { get; set; }
}