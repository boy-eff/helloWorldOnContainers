using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Words.DataAccess.Models;

public class WordCollectionRating
{
    public int Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
    public int UserId { get; set; }
    public User User { get; set; }
    public int CollectionId { get; set; }
    public WordCollection Collection { get; set; }
    [Range(1, 5)]
    public int Rating { get; set; }
}