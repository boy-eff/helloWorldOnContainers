using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Words.DataAccess.Models;

public class WordCollectionRate
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
    public User User { get; set; }
    public int CollectionId { get; set; }
    public WordCollection Collection { get; set; }
    [Range(1, 5)]
    public int Rate { get; set; }
}