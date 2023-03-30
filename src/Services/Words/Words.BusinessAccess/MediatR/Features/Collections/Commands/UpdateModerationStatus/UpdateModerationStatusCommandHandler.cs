using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Exceptions;
using Words.BusinessAccess.Extensions;
using Words.BusinessAccess.Helpers;
using Words.BusinessAccess.Options;
using Words.DataAccess;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.MediatR.Features.Collections.Commands.UpdateModerationStatus;

public class UpdateModerationStatusCommandHandler : IRequestHandler<UpdateModerationStatusCommand, int>
{
    private readonly WordsDbContext _dbContext;
    private readonly ILogger<UpdateModerationStatusCommandHandler> _logger;
    private readonly IDistributedCache _cache;
    private readonly IOptions<WordsRedisCacheOptions> _options;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateModerationStatusCommandHandler(WordsDbContext dbContext, IHttpContextAccessor httpContextAccessor, ILogger<UpdateModerationStatusCommandHandler> logger, IDistributedCache cache, IOptions<WordsRedisCacheOptions> options)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _cache = cache;
        _options = options;
    }

    public async Task<int> Handle(UpdateModerationStatusCommand request, CancellationToken cancellationToken)
    {
        WordCollection collection;
        var cacheKey = CacheHelper.GetCacheKeyForWordCollection(request.ModerationDto.WordCollectionId);
        var isCollectionInCache = _cache.TryGetValue(cacheKey, out collection);
        
        if (!isCollectionInCache)
        {
            collection = await _dbContext.Collections.FindAsync(request.ModerationDto.WordCollectionId);
        }

        if (collection is null)
        {
            _logger.LogInformation("Collection with id {CollectionId was not found}", request.ModerationDto.WordCollectionId);
            throw new NotFoundException("Collection was not found");
        }

        collection.ActualModerationStatus = request.ModerationDto.ModerationStatusId;

        var moderation = request.ModerationDto.Adapt<WordCollectionModeration>();
        moderation.ModeratorId = _httpContextAccessor.HttpContext.User.GetUserId();
        
        await _dbContext.WordCollectionModerations.AddAsync(moderation, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Moderation status for collection with id {CollectionId} was successfully updated to {ModerationStatus}",
            collection.Id, moderation.ModerationStatus);
        
        if (!isCollectionInCache)
        {
            return moderation.Id;
        }
        
        var cacheOptions = new DistributedCacheEntryOptions()
            { SlidingExpiration = TimeSpan.FromMinutes(_options.Value.SlidingExpirationTimeInMinutes) };
        await _cache.SetAsync(cacheKey, collection, cacheOptions);
        
        _logger.LogInformation("Updated word collection with id {CollectionId} was successfully cached after updating", collection.Id);

        return moderation.Id;
    }
}