using Words.DataAccess.Models;

namespace Words.BusinessAccess.Dtos;

public class WordCollectionTestPassInformationResponseDto
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public int WordCollectionId { get; set; }

    public ICollection<PassedWordCollectionTestQuestionResponseDto> Questions { get; set; }
    
    public int TotalQuestions { get; set; }
    public int CorrectAnswersAmount { get; set; }
}