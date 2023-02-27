using MediatR;
using Words.BusinessAccess.Dtos;

namespace Words.BusinessAccess.Features.Collections.Queries.Get;

public class GetWordCollectionsQuery : IRequest<IEnumerable<WordCollectionDto>>
{
    
}
