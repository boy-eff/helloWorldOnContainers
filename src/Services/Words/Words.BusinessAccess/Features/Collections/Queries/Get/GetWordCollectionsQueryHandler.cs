using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Words.BusinessAccess.Dtos;
using Words.DataAccess;

namespace Words.BusinessAccess.Features.Collections.Queries.Get;

public class GetWordCollectionsQueryHandler : IRequestHandler<GetWordCollectionsQuery, IEnumerable<WordCollectionDto>>
{
    private readonly WordsDbContext _dbContext;

    public GetWordCollectionsQueryHandler(WordsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<WordCollectionDto>> Handle(GetWordCollectionsQuery request, CancellationToken cancellationToken)
    {
        var wordCollections = await _dbContext.Collections
            .Include(x => x.Words)
            .ThenInclude(x => x.Translations)
            .ToListAsync(cancellationToken: cancellationToken);
        return wordCollections.Adapt<List<WordCollectionDto>>();
    }
}