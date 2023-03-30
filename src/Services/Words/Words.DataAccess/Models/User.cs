using System.ComponentModel.DataAnnotations.Schema;
using Shared.Enums;

namespace Words.DataAccess.Models;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public string UserName { get; set; }
    public EnglishLevel EnglishLevel { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

    public ICollection<WordCollection> Collections { get; set; }
    public ICollection<WordCollectionRating> CollectionRatings { get; set; }
    public ICollection<UserWord> DictionaryWords { get; set; }
    public ICollection<WordCollectionTestPassInformation> TestsPassInformation { get; set; }
    public ICollection<WordCollectionModeration> Moderations { get; set; }
}