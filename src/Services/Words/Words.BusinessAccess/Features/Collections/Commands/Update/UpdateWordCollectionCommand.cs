using MediatR;
using Words.BusinessAccess.Dtos;

namespace Words.BusinessAccess.Features.Collections.Commands.Update;

public record UpdateWordCollectionCommand(WordCollectionDto WordCollectionDto) : IRequest<WordCollectionDto>;