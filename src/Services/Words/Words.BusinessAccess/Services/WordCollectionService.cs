using System.Security.Claims;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Words.BusinessAccess.Contracts;
using Words.BusinessAccess.Dtos;
using Words.BusinessAccess.Extensions;
using Words.DataAccess;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.Services;

public class WordCollectionService : IWordCollectionService
{
    private readonly WordsDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WordCollectionService(WordsDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<WordCollectionDto>> GetAsync()
    {
        var wordCollections = await _dbContext.Collections
            .Include(x => x.Words)
            .ThenInclude(x => x.Translations)
            .ToListAsync();
        return wordCollections.Adapt<List<WordCollectionDto>>();
    }
    
    public async Task<IEnumerable<WordCollectionDto>> GetByUserIdAsync(int userId)
    {
        var wordCollections = await _dbContext.Collections
            .Where(x => x.UserId == userId).ToListAsync();
        return wordCollections.Adapt<List<WordCollectionDto>>();
    }

    public async Task<int> InsertAsync(WordCollectionCreateDto wordCollectionCreateDto)
    {
        var wordCollection = wordCollectionCreateDto.Adapt<WordCollection>();
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        
        if (userId == 0)
        {
            return userId;
        }

        wordCollection.UserId = userId;
        await _dbContext.Collections.AddAsync(wordCollection);
        await _dbContext.SaveChangesAsync();
        return wordCollection.Id;
    }

    public async Task<int> UpdateAsync(WordCollectionDto wordCollectionDto)
    {
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        
        if (userId == 0)
        {
            return 0;
        }
        
        var existingWordCollection = await _dbContext.Collections
            .FirstOrDefaultAsync(x => x.Id == wordCollectionDto.Id && x.UserId == userId);
        
        if (existingWordCollection is null)
        {
            return 0;
        }
        
        var wordCollection = wordCollectionDto.Adapt(existingWordCollection);
        _dbContext.Collections.Update(wordCollection);
        await _dbContext.SaveChangesAsync();
        return wordCollection.Id;
    }

    public async Task<int> DeleteAsync(int id)
    {
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();

        if (userId == 0)
        {
            return 0;
        }
        var wordCollection = await _dbContext.Collections.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        
        if (wordCollection is null)
        {
            return 0;
        }

        _dbContext.Collections.Remove(wordCollection);
        await _dbContext.SaveChangesAsync();
        return id;
    }
}