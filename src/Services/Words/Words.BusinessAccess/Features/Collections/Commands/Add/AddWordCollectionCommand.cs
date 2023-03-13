using MediatR;
using Words.BusinessAccess.Dtos;
using Words.BusinessAccess.Dtos.WordCollection;

namespace Words.BusinessAccess.Features.Collections.Commands.Add;

public record AddWordCollectionCommand(WordCollectionRequestDto WordCollectionCreateDto) : IRequest<WordCollectionResponseDto>;
