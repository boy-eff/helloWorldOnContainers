using MediatR;
using Words.BusinessAccess.Dtos;

namespace Words.BusinessAccess.Features.Collections.Queries;

public class GetWordCollectionsQuery : IRequest<IEnumerable<WordCollectionDto>>
{
    
}
