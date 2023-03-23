using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<UpdateWordCollectionCommandHandler> _logger;

    public UpdateWordCollectionCommandHandler(IHttpContextAccessor httpContextAccessor,
        WordsDbContext dbContext, 
        IDistributedCache cache, 
        IOptions<WordsRedisCacheOptions> wordsRedisCacheOptions, 
        ILogger<UpdateWordCollectionCommandHandler> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
        _cache = cache;
        _wordsRedisCacheOptions = wordsRedisCacheOptions;
        _logger = logger;
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
            _logger.LogInformation("Updating failed: Collection with id {CollectionId} was not found", request.Id);
            throw new NotFoundException($"Collection with id {request.Id} was not found");
        }

        if (existingWordCollection.UserId != userId)
        {
            _logger.LogInformation("Updating failed: User with id {UserId} has no permission to update {CollectionId}", userId, request.Id);
            throw new ForbiddenException($"Cannot update collection with id {existingWordCollection.Id}");
        }

        var wordCollection = request.WordCollectionDto.Adapt(existingWordCollection);
        _dbContext.Collections.Update(wordCollection);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Word collection with id {CollectionId} was successfully updated", wordCollection.Id);

        if (!isCollectionInCache)
        {
            return wordCollection.Adapt<WordCollectionResponseDto>();
        }
        
        var cacheOptions = new DistributedCacheEntryOptions()
            { SlidingExpiration = TimeSpan.FromMinutes(_wordsRedisCacheOptions.Value.SlidingExpirationTimeInMinutes) };
        await _cache.SetAsync(cacheKey, wordCollection, cacheOptions);
        
        _logger.LogInformation("Updated word collection with id {CollectionId} was successfully cached after updating", wordCollection.Id);

        return wordCollection.Adapt<WordCollectionResponseDto>();
    }
}