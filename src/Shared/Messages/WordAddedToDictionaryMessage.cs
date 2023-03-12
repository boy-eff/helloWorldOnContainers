namespace Shared.Messages;

public class WordAddedToDictionaryMessage
{
    public int DictionaryOwnerId { get; set; }
    public int WordId { get; set; }
}