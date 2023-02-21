namespace Words.DataAccess.Models;

public class UserWord
{
    public int UserId { get; set; }
    public User User { get; set; }
    public int WordId { get; set; }
    public Word Word { get; set; }
}