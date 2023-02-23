using MediatR;
using Words.BusinessAccess.Dtos;

namespace Words.BusinessAccess.Features.Collections.Commands;

public record AddWordCollectionCommand(WordCollectionCreateDto WordCollectionCreateDto) : IRequest<WordCollectionDto>;
