using FluentValidation;
using Words.BusinessAccess.ModelValidators;

namespace Words.BusinessAccess.MediatR.Features.Collections.Commands.Update;

public class UpdateWordCollectionCommandValidator : AbstractValidator<UpdateWordCollectionCommand>
{
    public UpdateWordCollectionCommandValidator()
    {
        RuleFor(x => x.WordCollectionDto)
            .SetValidator(new WordCollectionRequestDtoValidator());
    }
}