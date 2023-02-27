using Words.BusinessAccess.Dtos;

namespace Words.BusinessAccess.Contracts;

public interface IRatingService
{
    Task<List<CollectionRatingDto>> GetByCollectionIdAsync(int collectionId);
    Task<int> InsertAsync(CollectionRatingCreateDto ratingCreateDto);
    Task<int> UpdateAsync(CollectionRatingDto ratingDto);
    Task<int> DeleteAsync(int id);
}