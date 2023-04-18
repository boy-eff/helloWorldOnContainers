using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Words.BusinessAccess.Dtos.WordCollection;
using Words.BusinessAccess.Extensions;
using Words.BusinessAccess.Models;
using Words.DataAccess;

namespace Words.BusinessAccess.MediatR.Features.Collections.Queries.Get;

public class GetWordCollectionsQueryHandler : IRequestHandler<GetWordCollectionsQuery, PaginationResult<WordCollectionResponseDto>>
{
    private readonly WordsDbContext _dbContext;

    public GetWordCollectionsQueryHandler(WordsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PaginationResult<WordCollectionResponseDto>> Handle(GetWordCollectionsQuery request, CancellationToken cancellationToken)
    {
        var paginationParams = request.PaginationParameters;

        var totalCount = await _dbContext.Collections.CountAsync(cancellationToken: cancellationToken);
        
        var wordCollections = await _dbContext.Collections
            .GetPage(paginationParams)
            .ToListAsync(cancellationToken: cancellationToken);

        return new PaginationResult<WordCollectionResponseDto>()
        {
            TotalCount = totalCount,
            Value = wordCollections.Adapt<IEnumerable<WordCollectionResponseDto>>()
        };
    }
}