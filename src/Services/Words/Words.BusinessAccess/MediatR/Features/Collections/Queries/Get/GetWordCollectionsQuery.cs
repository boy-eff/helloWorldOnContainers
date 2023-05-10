using MediatR;
using Words.BusinessAccess.Dtos.WordCollection;
using Words.BusinessAccess.Models;

namespace Words.BusinessAccess.MediatR.Features.Collections.Queries.Get;

public record GetWordCollectionsQuery(PaginationParameters PaginationParameters, FilteringParameters FilteringParameters) : IRequest<PaginationResult<WordCollectionResponseDto>>;
