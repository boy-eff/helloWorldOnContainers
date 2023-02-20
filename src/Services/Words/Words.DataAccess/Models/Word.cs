namespace Words.DataAccess.Models;

public class Word : BaseEntity
{
    public int Id { get; set; }
    public int WordCollectionId { get; set; }
    public string Value { get; set; }
}