using FluentValidation;
using Words.BusinessAccess.Dtos;

namespace Words.BusinessAccess.ModelValidators;

public class WordCreateDtoValidator : AbstractValidator<WordCreateDto>
{
    public WordCreateDtoValidator()
    {
        RuleFor(x => x.Value)
            .NotNull()
            .NotEmpty()
            .WithMessage("Word value should not be empty");
        RuleFor(x => x.Translations)
            .NotNull()
            .NotEmpty()
            .WithMessage("Provide at least 1 translation of the word");
        RuleForEach(x => x.Translations)
            .SetValidator(new WordTranslationCreateDtoValidator());
    }
}