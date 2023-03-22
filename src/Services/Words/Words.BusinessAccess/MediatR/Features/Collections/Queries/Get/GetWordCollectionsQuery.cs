using MediatR;
using Words.BusinessAccess.Dtos.WordCollection;

namespace Words.BusinessAccess.MediatR.Features.Collections.Queries.Get;

public record GetWordCollectionsQuery() : IRequest<IEnumerable<WordCollectionResponseDto>>;
