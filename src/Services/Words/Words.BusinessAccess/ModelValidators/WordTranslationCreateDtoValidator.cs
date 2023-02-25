using FluentValidation;
using Words.BusinessAccess.Dtos;

namespace Words.BusinessAccess.ModelValidators;

public class WordTranslationCreateDtoValidator : AbstractValidator<WordTranslationCreateDto>
{
    public WordTranslationCreateDtoValidator()
    {
        RuleFor(x => x.Translation)
            .NotNull()
            .NotEmpty()
            .WithMessage("Word translation could not be empty");
    }
}