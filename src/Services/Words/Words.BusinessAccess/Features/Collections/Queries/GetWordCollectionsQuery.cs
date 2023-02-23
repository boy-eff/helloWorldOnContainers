using MediatR;
using Words.BusinessAccess.Dtos;

namespace Words.BusinessAccess.Features.Collections.Queries;

public record GetWordCollectionsQuery() : IRequest<IEnumerable<WordCollectionDto>>;
