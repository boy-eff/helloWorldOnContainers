namespace Words.BusinessAccess.Dtos;

public class WordCreateDto
{
    public string Value { get; set; }
    public List<WordTranslationCreateDto> Translations { get; set; }
}