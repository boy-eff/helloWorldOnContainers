using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Exceptions;
using Words.BusinessAccess.Contracts;
using Words.BusinessAccess.Dtos.WordCollection;
using Words.BusinessAccess.Extensions;
using Words.DataAccess;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.MediatR.Features.Collections.Queries.GetById;

public class GetWordCollectionByIdQueryHandler : IRequestHandler<GetWordCollectionByIdQuery, WordCollectionResponseDto>
{
    private readonly WordsDbContext _dbContext;
    private readonly IViewsCounterService _viewsCounterService;
    private readonly IDistributedCache _cache;

    public GetWordCollectionByIdQueryHandler(WordsDbContext dbContext, IViewsCounterService viewsCounterService, IDistributedCache cache)
    {
        _dbContext = dbContext;
        _viewsCounterService = viewsCounterService;
        _cache = cache;
    }

    public async Task<WordCollectionResponseDto> Handle(GetWordCollectionByIdQuery request, CancellationToken cancellationToken)
    {
        var wordCollectionName = nameof(WordCollection);
        WordCollection collection;
        var isCacheHit = _cache.TryGetWordCollection(request.Id, out collection);

        if (!isCacheHit)
        {
            collection = await _dbContext.Collections
                .Include(x => x.Words)
                .ThenInclude(x => x.Translations)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        }
        
        if (collection is null)
        {
            throw new NotFoundException("Collection is not found");
        }
        
        _viewsCounterService.IncrementViewsInCollection(collection.Id);
        return collection.Adapt<WordCollectionResponseDto>();
    }
}