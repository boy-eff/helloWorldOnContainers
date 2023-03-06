namespace Words.BusinessAccess.Dtos.CollectionRating;

public class CollectionRatingResponseDto
{
    public int Id { get; set; }
    public int CollectionId { get; set; }
    public int UserId { get; set; }
    public int Rating { get; set; }
}