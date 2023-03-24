using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;
using Words.BusinessAccess.Contracts;
using Words.BusinessAccess.Dtos.WordCollection;
using Words.BusinessAccess.Extensions;
using Words.BusinessAccess.Helpers;
using Words.DataAccess;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.MediatR.Features.Collections.Queries.GetById;

public class GetWordCollectionByIdQueryHandler : IRequestHandler<GetWordCollectionByIdQuery, WordCollectionResponseDto>
{
    private readonly WordsDbContext _dbContext;
    private readonly IViewsCounterService _viewsCounterService;
    private readonly IDistributedCache _cache;
    private readonly ILogger<GetWordCollectionByIdQueryHandler> _logger;

    public GetWordCollectionByIdQueryHandler(WordsDbContext dbContext, 
        IViewsCounterService viewsCounterService, 
        IDistributedCache cache, 
        ILogger<GetWordCollectionByIdQueryHandler> logger)
    {
        _dbContext = dbContext;
        _viewsCounterService = viewsCounterService;
        _cache = cache;
        _logger = logger;
    }

    public async Task<WordCollectionResponseDto> Handle(GetWordCollectionByIdQuery request, CancellationToken cancellationToken)
    {
        WordCollection collection;
        var cacheKey = CacheHelper.GetCacheKeyForWordCollection(request.Id);
        var isCollectionInCache = _cache.TryGetValue(cacheKey, out collection);

        if (!isCollectionInCache)
        {
            collection = await _dbContext.Collections
                .Include(x => x.Words)
                .ThenInclude(x => x.Translations)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        }
        
        if (collection is null)
        {
            _logger.LogInformation("Failed to retrieve: Word collection with id {CollectionId} was not found", request.Id);
            throw new NotFoundException($"Word collection with id {request.Id} was not found");
        }
        
        _logger.LogInformation("Word collection with id {CollectionId} was successfully retrieved", request.Id);
        
        _viewsCounterService.IncrementViewsInCollection(collection.Id);
        return collection.Adapt<WordCollectionResponseDto>();
    }
}