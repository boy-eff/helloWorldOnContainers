using Words.BusinessAccess.Dtos;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.Contracts;

public interface ICollectionService
{
    Task<List<WordCollectionDto>> GetAsync();
    Task<List<WordCollectionDto>> GetByUserIdAsync(int userId);
    Task<int> InsertAsync(WordCollectionCreateDto wordCollectionCreateDto);
    Task<int> UpdateAsync(WordCollectionDto wordCollectionDto);
    Task<int> DeleteAsync(int id);
}