namespace Words.BusinessAccess.Options;

public class WordsRedisCacheOptions
{
    public const string Section = "Redis";
    
    public int CachedCollectionsCount { get; set; }
    public int SlidingExpirationTimeInMinutes { get; set; }
}