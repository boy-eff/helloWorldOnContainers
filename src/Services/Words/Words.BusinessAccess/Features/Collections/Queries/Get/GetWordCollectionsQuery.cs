using MediatR;
using Words.BusinessAccess.Dtos;
using Words.BusinessAccess.Dtos.WordCollection;

namespace Words.BusinessAccess.Features.Collections.Queries.Get;

public record GetWordCollectionsQuery() : IRequest<IEnumerable<WordCollectionResponseDto>>;
