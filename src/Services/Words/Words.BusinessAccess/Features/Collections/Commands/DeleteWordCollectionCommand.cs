using MediatR;

namespace Words.BusinessAccess.Features.Collections.Commands;

public record DeleteWordCollectionCommand(int WordCollectionId) : IRequest<int?>;