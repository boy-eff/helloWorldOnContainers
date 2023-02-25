using FluentValidation;
using Words.BusinessAccess.Dtos;

namespace Words.BusinessAccess.ModelValidators;

public class WordTranslationDtoValidator : AbstractValidator<WordTranslationDto>
{
    public WordTranslationDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(0)
            .WithMessage("Translation id cannot be 0");
        RuleFor(x => x.Translation)
            .NotNull()
            .NotEmpty()
            .WithMessage("Word translation could not be empty");
    }
}