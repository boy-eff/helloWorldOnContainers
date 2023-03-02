using MediatR;
using Words.BusinessAccess.Dtos;

namespace Words.BusinessAccess.Features.Collections.Queries.GetById;

public record GetWordCollectionByIdQuery(int Id) : IRequest<WordCollectionDto>;