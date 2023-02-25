using FluentValidation;
using Words.BusinessAccess.ModelValidators;

namespace Words.BusinessAccess.Features.Collections.Commands.Add;

public class AddWordCollectionCommandValidator : AbstractValidator<AddWordCollectionCommand>
{
    public AddWordCollectionCommandValidator()
    {
        RuleFor(x => x.WordCollectionCreateDto)
            .SetValidator(new WordCollectionCreateDtoValidator());
    }
}