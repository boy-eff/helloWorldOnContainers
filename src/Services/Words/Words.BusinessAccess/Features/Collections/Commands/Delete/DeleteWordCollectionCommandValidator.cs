using FluentValidation;

namespace Words.BusinessAccess.Features.Collections.Commands.Delete;

public class DeleteWordCollectionCommandValidator : AbstractValidator<DeleteWordCollectionCommand>
{
    public DeleteWordCollectionCommandValidator()
    {
        RuleFor(x => x.WordCollectionId)
            .NotNull()
            .NotEqual(0);
    }
}