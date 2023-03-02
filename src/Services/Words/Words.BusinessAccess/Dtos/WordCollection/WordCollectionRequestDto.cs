using Shared.Enums;

namespace Words.BusinessAccess.Dtos.WordCollection;

public class WordCollectionRequestDto
{
    public string Name { get; set; }
    public EnglishLevel EnglishLevel { get; set; }
    public List<WordCreateDto> Words { get; set; }
}