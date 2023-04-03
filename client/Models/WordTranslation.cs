namespace Words.DataAccess.Models;

public class WordTranslation
{
    public int Id { get; set; }
    public string Translation { get; set; }
    
    public int WordId { get; set; }
    public Word Word { get; set; }
}