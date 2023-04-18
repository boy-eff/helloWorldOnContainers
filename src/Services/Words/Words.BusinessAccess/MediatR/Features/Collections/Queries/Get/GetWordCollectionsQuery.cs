using MediatR;
using Words.BusinessAccess.Dtos.WordCollection;
using Words.BusinessAccess.Models;
using Words.BusinessAccess.Pagination;

namespace Words.BusinessAccess.MediatR.Features.Collections.Queries.Get;

public record GetWordCollectionsQuery(PaginationParameters PaginationParameters) : IRequest<PaginationResult<WordCollectionResponseDto>>;
