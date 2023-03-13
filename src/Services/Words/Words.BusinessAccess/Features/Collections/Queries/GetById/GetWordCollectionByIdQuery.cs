using MediatR;
using Words.BusinessAccess.Dtos;
using Words.BusinessAccess.Dtos.WordCollection;

namespace Words.BusinessAccess.Features.Collections.Queries.GetById;

public record GetWordCollectionByIdQuery(int Id) : IRequest<WordCollectionResponseDto>;