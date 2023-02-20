using Shared.Enums;

namespace Words.BusinessAccess.Dtos;

public class WordCollectionDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public EnglishLevel EnglishLevel { get; set; }
}