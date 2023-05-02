using FluentValidation;
using Words.BusinessAccess.Dtos;

namespace Words.BusinessAccess.ModelValidators;

public class WordCreateDtoValidator : AbstractValidator<WordCreateDto>
{
    public WordCreateDtoValidator()
    {
        const int minimumLength = 1;
        RuleFor(x => x.Value)
            .NotNull()
            .NotEmpty()
            .WithMessage("Word value should not be empty");
        RuleFor(x => x.Translations)
            .NotNull()
            .NotEmpty()
            .WithMessage("Provide at least 1 translation of the word");
        RuleForEach(x => x.Translations)
            .NotEmpty()
            .WithMessage("Translation cannot be empty");
    }
}