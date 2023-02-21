namespace Words.BusinessAccess.Dtos;

public class CollectionRatingDto
{
    public int Id { get; set; }
    public int CollectionId { get; set; }
    public int UserId { get; set; }
    public int Rating { get; set; }
}