namespace Words.BusinessAccess.Dtos;

public class WordDto
{
    public int Id { get; set; }
    public string Value { get; set; }
    public List<WordTranslationDto> Translations { get; set; }
}