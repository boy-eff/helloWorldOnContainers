namespace Words.BusinessAccess.Dtos;

public class WordCollectionTestQuestionDto
{
    public string Word { get; set; }
    public IEnumerable<string> AnswerOptions { get; set; }
}