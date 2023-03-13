using MediatR;
using Words.BusinessAccess.Dtos.WordCollection;

namespace Words.BusinessAccess.MediatR.Features.Collections.Commands.Add;

public record AddWordCollectionCommand(WordCollectionRequestDto WordCollectionCreateDto) : IRequest<WordCollectionResponseDto>;
