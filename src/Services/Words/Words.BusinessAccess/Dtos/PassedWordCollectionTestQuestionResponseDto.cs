namespace Words.BusinessAccess.Dtos;

public class PassedWordCollectionTestQuestionResponseDto
{
    public int Id { get; set; }
    public int WordCollectionTestPassInformationId { get; set; }
    public string Word { get; set; }
    public string CorrectAnswer { get; set; }
    public string UserAnswer { get; set; }
}