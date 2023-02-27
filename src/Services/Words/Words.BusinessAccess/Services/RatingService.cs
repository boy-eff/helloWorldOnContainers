using System.Security.Claims;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Words.BusinessAccess.Contracts;
using Words.BusinessAccess.Dtos;
using Words.BusinessAccess.Extensions;
using Words.DataAccess;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.Services;

public class RatingService : IRatingService
{
    private readonly WordsDbContext _context;
    private readonly ClaimsPrincipal _user;

    public RatingService(WordsDbContext context, ClaimsPrincipal user)
    {
        _context = context;
        _user = user;
    }

    public async Task<List<CollectionRatingDto>> GetByCollectionIdAsync(int collectionId)
    {
        var ratings = await _context.WordCollectionRatings
            .Where(x => x.CollectionId == collectionId)
            .ToListAsync();
        return ratings.Adapt<List<CollectionRatingDto>>();
    }

    public async Task<int> InsertAsync(CollectionRatingCreateDto ratingCreateDto)
    {
        var userId = _user.GetUserId();
        
        if (userId == 0)
        {
            return userId;
        }
        
        var existingRating = await _context.WordCollectionRatings
            .FirstOrDefaultAsync(x => x.CollectionId == ratingCreateDto.CollectionId && x.UserId == userId);
        
        if (existingRating is not null)
        {
            return 0;
        }
        
        var rating = ratingCreateDto.Adapt<WordCollectionRating>();
        await _context.WordCollectionRatings.AddAsync(rating);
        await _context.SaveChangesAsync();
        return rating.Id;
    }

    public async Task<int> UpdateAsync(CollectionRatingDto ratingDto)
    {
        var userId = _user.GetUserId();
        
        if (userId == 0)
        {
            return 0;
        }
        
        var existingRating = await _context.WordCollectionRatings
            .FirstOrDefaultAsync(x => x.Id == ratingDto.Id && x.UserId == userId);
        
        if (existingRating is null)
        {
            return 0;
        }
        
        var rating = ratingDto.Adapt(existingRating);
        _context.WordCollectionRatings.Update(rating);
        await _context.SaveChangesAsync();
        return rating.Id;
    }

    public async Task<int> DeleteAsync(int id)
    {
        var userId = _user.GetUserId();

        if (userId == 0)
        {
            return 0;
        }
        
        var rating = await _context.WordCollectionRatings
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        
        if (rating is null)
        {
            return 0;
        }

        _context.WordCollectionRatings.Remove(rating);
        await _context.SaveChangesAsync();
        return id;
    }
}