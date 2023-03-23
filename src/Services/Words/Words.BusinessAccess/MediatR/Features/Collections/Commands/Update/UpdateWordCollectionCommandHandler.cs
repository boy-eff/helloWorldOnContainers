using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Shared.Exceptions;
using Words.BusinessAccess.Dtos.WordCollection;
using Words.BusinessAccess.Extensions;
using Words.BusinessAccess.Helpers;
using Words.BusinessAccess.Options;
using Words.DataAccess;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.MediatR.Features.Collections.Commands.Update;

public class UpdateWordCollectionCommandHandler : IRequestHandler<UpdateWordCollectionCommand, WordCollectionResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly WordsDbContext _dbContext;
    private readonly IDistributedCache _cache;
    private readonly IOptions<WordsRedisCacheOptions> _wordsRedisCacheOptions;

    public UpdateWordCollectionCommandHandler(IHttpContextAccessor httpContextAccessor,
        WordsDbContext dbContext, IDistributedCache cache, IOptions<WordsRedisCacheOptions> wordsRedisCacheOptions)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
        _cache = cache;
        _wordsRedisCacheOptions = wordsRedisCacheOptions;
    }

    public async Task<WordCollectionResponseDto> Handle(UpdateWordCollectionCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor?.HttpContext?.User.GetUserId();
        WordCollection existingWordCollection;
        var cacheKey = CacheHelper.GetCacheKeyForWordCollection(request.Id);
        var isCollectionInCache = _cache.TryGetValue(cacheKey, out existingWordCollection);
        
        if (!isCollectionInCache)
        {
            existingWordCollection = await _dbContext.Collections
                .FirstOrDefaultAsync(x => x.Id == request.Id,
                    cancellationToken: cancellationToken);
        }
        
        if (existingWordCollection is null)
        {
            throw new NotFoundException($"Collection with id {request.Id} is not found");
        }

        if (existingWordCollection.UserId != userId)
        {
            throw new ForbiddenException($"Cannot update collection with id {existingWordCollection.Id}");
        }

        var wordCollection = request.WordCollectionDto.Adapt(existingWordCollection);
        _dbContext.Collections.Update(wordCollection);
        await _dbContext.SaveChangesAsync(cancellationToken);

        if (!isCollectionInCache)
        {
            return wordCollection.Adapt<WordCollectionResponseDto>();
        }
        
        var cacheOptions = new DistributedCacheEntryOptions()
            { SlidingExpiration = TimeSpan.FromMinutes(_wordsRedisCacheOptions.Value.SlidingExpirationTimeInMinutes) };
        await _cache.SetAsync(cacheKey, wordCollection, cacheOptions);

        return wordCollection.Adapt<WordCollectionResponseDto>();
    }
}