using MediatR;

namespace Words.BusinessAccess.MediatR.Features.Collections.Commands.Delete;

public record DeleteWordCollectionCommand(int WordCollectionId) : IRequest<int?>;