using MediatR;
using Words.BusinessAccess.Dtos;

namespace Words.BusinessAccess.Features.Collections.Commands;

public record UpdateWordCollectionCommand(WordCollectionDto WordCollectionDto) : IRequest<WordCollectionDto>;