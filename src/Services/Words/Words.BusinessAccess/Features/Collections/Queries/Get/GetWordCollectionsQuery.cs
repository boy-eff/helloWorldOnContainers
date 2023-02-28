using MediatR;
using Words.BusinessAccess.Dtos;

namespace Words.BusinessAccess.Features.Collections.Queries.Get;

public record GetWordCollectionsQuery() : IRequest<IEnumerable<WordCollectionDto>>;
