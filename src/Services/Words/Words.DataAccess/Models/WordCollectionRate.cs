using System.ComponentModel.DataAnnotations;

namespace Words.DataAccess.Models;

public class WordCollectionRate : BaseEntity
{
    public int UserId { get; set; }
    public int CollectionId { get; set; }
    [Range(1, 5)]
    public int Rate { get; set; }
}