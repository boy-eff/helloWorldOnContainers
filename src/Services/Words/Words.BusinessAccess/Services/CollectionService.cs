using Mapster;
using Microsoft.EntityFrameworkCore;
using Words.BusinessAccess.Contracts;
using Words.BusinessAccess.Dtos;
using Words.DataAccess;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.Services;

public class CollectionService : ICollectionService
{
    private readonly WordsDbContext _context;

    public CollectionService(WordsDbContext context)
    {
        _context = context;
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
        wordCollection.UserId = 1;
        await _context.Collections.AddAsync(wordCollection);
        await _context.SaveChangesAsync();
        return wordCollection.Id;
    }

    public async Task<int> UpdateAsync(WordCollectionDto wordCollectionDto)
    {
        var collection = wordCollectionDto.Adapt<WordCollection>();
        collection.UserId = 1;
        _context.Collections.Update(collection);
        await _context.SaveChangesAsync();
        return wordCollectionDto.Id;
    }

    public async Task<int> DeleteAsync(int id)
    {
        var collection = await _context.Collections.FirstOrDefaultAsync(x => x.Id == id);
        if (collection is null)
        {
            return 0;
        }

        _context.Collections.Remove(collection);
        await _context.SaveChangesAsync();
        return id;
    }
}