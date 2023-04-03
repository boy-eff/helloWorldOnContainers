namespace Words.DataAccess.Models;

public class Word
{
    public int Id { get; set; }
    public string Value { get; set; }
    
    public int WordCollectionId { get; set; }
    public IList<WordTranslation> Translations { get; set; }
}