using FluentValidation;
using Words.BusinessAccess.Dtos;

namespace Words.BusinessAccess.ModelValidators;

public class WordCollectionModerationDtoValidator : AbstractValidator<WordCollectionModerationDto>
{
    public WordCollectionModerationDtoValidator()
    {
        RuleFor(x => x.ModerationStatusId)
            .IsInEnum();
        RuleFor(x => x.Review)
            .MaximumLength(100);
        RuleFor(x => x.WordCollectionId)
            .NotNull();
    }
}