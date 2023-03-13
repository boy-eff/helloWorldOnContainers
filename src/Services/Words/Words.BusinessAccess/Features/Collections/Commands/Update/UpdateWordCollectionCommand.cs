using MediatR;
using Words.BusinessAccess.Dtos;
using Words.BusinessAccess.Dtos.WordCollection;

namespace Words.BusinessAccess.Features.Collections.Commands.Update;

public record UpdateWordCollectionCommand(int Id, WordCollectionRequestDto WordCollectionDto) : IRequest<WordCollectionResponseDto>;