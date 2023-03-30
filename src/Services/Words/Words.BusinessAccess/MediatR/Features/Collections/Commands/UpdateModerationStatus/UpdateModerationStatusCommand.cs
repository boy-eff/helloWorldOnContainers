using MediatR;
using Words.BusinessAccess.Dtos;

namespace Words.BusinessAccess.MediatR.Features.Collections.Commands.UpdateModerationStatus;

public record UpdateModerationStatusCommand(WordCollectionModerationDto ModerationDto) : IRequest<int>;