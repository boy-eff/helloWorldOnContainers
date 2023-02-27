using Shared.Enums;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.Dtos;

public class WordCollectionCreateDto
{
    public string Name { get; set; }
    public EnglishLevel EnglishLevel { get; set; }
    
    public List<WordCreateDto> Words { get; set; }
}