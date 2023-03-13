using MediatR;
using Words.BusinessAccess.Dtos.WordCollection;

namespace Words.BusinessAccess.MediatR.Features.Collections.Queries.GetById;

public record GetWordCollectionByIdQuery(int Id) : IRequest<WordCollectionResponseDto>;