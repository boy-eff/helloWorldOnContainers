using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;
using Shared.Extensions;
using Words.BusinessAccess.Extensions;
using Words.BusinessAccess.Helpers;
using Words.DataAccess;

namespace Words.BusinessAccess.MediatR.Features.Collections.Commands.Delete;

public class DeleteWordCollectionCommandHandler : IRequestHandler<DeleteWordCollectionCommand, int?>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly WordsDbContext _dbContext;
    private readonly ILogger<DeleteWordCollectionCommandHandler> _logger;
    private readonly IDistributedCache _cache;

    public DeleteWordCollectionCommandHandler(IHttpContextAccessor httpContextAccessor,
        WordsDbContext dbContext, 
        ILogger<DeleteWordCollectionCommandHandler> logger, IDistributedCache cache)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
        _logger = logger;
        _cache = cache;
    }

    public async Task<int?> Handle(DeleteWordCollectionCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor?.HttpContext?.User.GetUserId();

        var wordCollection = await _dbContext.Collections.FirstOrDefaultAsync(x => x.Id == request.WordCollectionId,
            cancellationToken: cancellationToken);
        
        if (wordCollection is null)
        {
            _logger.LogInformation("Deletion failed: Collection with id {CollectionId} was not found", request.WordCollectionId);
            throw new NotFoundException($"Collection with id {request.WordCollectionId} was not found");
        }

        if (wordCollection.UserId != userId)
        {
            _logger.LogInformation("Deletion failed: User with id {UserId} has no permission to delete {CollectionId}", userId, request.WordCollectionId);
            throw new ForbiddenException($"Cannot delete collection with id {wordCollection.Id}");
        }

        _dbContext.Collections.Remove(wordCollection);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Word collection with id {CollectionId} was successfully deleted", wordCollection.Id);

        var cacheKey = CacheHelper.GetCacheKeyForWordCollection(wordCollection.Id);
        await _cache.RemoveAsync(cacheKey, cancellationToken);
        return request.WordCollectionId;
    }
}