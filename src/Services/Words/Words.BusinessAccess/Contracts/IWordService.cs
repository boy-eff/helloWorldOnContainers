using Words.BusinessAccess.Dtos;

namespace Words.BusinessAccess.Contracts;

public interface IWordService
{
    Task<List<WordDto>> GetWordsByUserDictionaryAsync();
}