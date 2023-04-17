using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Words.BusinessAccess.Dtos.WordCollection;
using Words.BusinessAccess.Extensions;
using Words.DataAccess;

namespace Words.BusinessAccess.MediatR.Features.Collections.Queries.Get;

public class GetWordCollectionsQueryHandler : IRequestHandler<GetWordCollectionsQuery, IEnumerable<WordCollectionResponseDto>>
{
    private readonly WordsDbContext _dbContext;

    public GetWordCollectionsQueryHandler(WordsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<WordCollectionResponseDto>> Handle(GetWordCollectionsQuery request, CancellationToken cancellationToken)
    {
        var paginationParams = request.PaginationParameters;
        var wordCollections = await _dbContext.Collections
            .Include(x => x.Words)
            .ThenInclude(x => x.Translations)
            .GetPage(paginationParams)
            .ToListAsync(cancellationToken: cancellationToken);
        return wordCollections.Adapt<List<WordCollectionResponseDto>>();
    }
}