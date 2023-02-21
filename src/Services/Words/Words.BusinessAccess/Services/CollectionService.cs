using System.Security.Claims;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Words.BusinessAccess.Contracts;
using Words.BusinessAccess.Dtos;
using Words.BusinessAccess.Extensions;
using Words.DataAccess;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.Services;

public class CollectionService : ICollectionService
{
    private readonly WordsDbContext _context;
    private readonly ClaimsPrincipal _user;

    public CollectionService(WordsDbContext context, ClaimsPrincipal user)
    {
        _context = context;
        _user = user;
    }

    public async Task<List<WordCollectionDto>> GetAsync()
    {
        var collections = await _context.Collections
            .Include(x => x.Words)
            .ThenInclude(x => x.Translations)
            .ToListAsync();
        return collections.Adapt<List<WordCollectionDto>>();
    }

    public async Task<List<WordCollectionDto>> GetByUserIdAsync(int userId)
    {
        var collections = await _context.Collections
            .Where(x => x.UserId == userId).ToListAsync();
        return collections.Adapt<List<WordCollectionDto>>();
    }

    public async Task<int> InsertAsync(WordCollectionCreateDto wordCollectionCreateDto)
    {
        var wordCollection = wordCollectionCreateDto.Adapt<WordCollection>();
        var userId = _user.GetUserId();
        
        if (userId == 0)
        {
            return userId;
        }

        wordCollection.UserId = userId;
        await _context.Collections.AddAsync(wordCollection);
        await _context.SaveChangesAsync();
        return wordCollection.Id;
    }

    public async Task<int> UpdateAsync(WordCollectionDto wordCollectionDto)
    {
        var userId = _user.GetUserId();
        
        if (userId == 0)
        {
            return 0;
        }
        
        var existingCollection = await _context.Collections
            .FirstOrDefaultAsync(x => x.Id == wordCollectionDto.Id && x.UserId == userId);
        
        if (existingCollection is null)
        {
            return 0;
        }
        
        var collection = wordCollectionDto.Adapt(existingCollection);
        _context.Collections.Update(collection);
        await _context.SaveChangesAsync();
        return collection.Id;
    }

    public async Task<int> DeleteAsync(int id)
    {
        var userId = _user.GetUserId();

        if (userId == 0)
        {
            return 0;
        }
        var collection = await _context.Collections.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        
        if (collection is null)
        {
            return 0;
        }

        _context.Collections.Remove(collection);
        await _context.SaveChangesAsync();
        return id;
    }
}