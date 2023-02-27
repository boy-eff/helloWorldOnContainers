using FluentValidation;
using Words.BusinessAccess.Dtos;

namespace Words.BusinessAccess.ModelValidators;

public class WordDtoValidator : AbstractValidator<WordDto>
{
    public WordDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(0)
            .WithMessage("Id cannot be 0");
        RuleFor(x => x.Value)
            .NotNull()
            .NotEmpty()
            .WithMessage("Word value could not be empty");
        RuleFor(x => x.Translations)
            .NotNull()
            .NotEmpty()
            .WithMessage("Provide at least 1 translation of the word");
        RuleForEach(x => x.Translations)
            .SetValidator(new WordTranslationDtoValidator());
    }
}