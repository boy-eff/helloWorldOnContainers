using FluentValidation;
using Words.BusinessAccess.ModelValidators;

namespace Words.BusinessAccess.MediatR.Features.Collections.Commands.UpdateModerationStatus;

public class UpdateModerationStatusCommandValidator : AbstractValidator<UpdateModerationStatusCommand>
{
    public UpdateModerationStatusCommandValidator()
    {
        RuleFor(x => x.ModerationDto)
            .SetValidator(new WordCollectionModerationDtoValidator());
    }
}