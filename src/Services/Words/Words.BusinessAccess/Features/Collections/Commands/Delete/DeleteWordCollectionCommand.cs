using MediatR;

namespace Words.BusinessAccess.Features.Collections.Commands.Delete;

public record DeleteWordCollectionCommand(int WordCollectionId) : IRequest<int?>;