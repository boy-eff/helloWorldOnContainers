using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Words.BusinessAccess.Dtos.WordCollection;

public class WordCollectionRequestDto
{
    public string Name { get; set; }
    public EnglishLevel EnglishLevel { get; set; }
    public IFormFile Image { get; set; }
    public IEnumerable<WordCreateDto> Words { get; set; }
}