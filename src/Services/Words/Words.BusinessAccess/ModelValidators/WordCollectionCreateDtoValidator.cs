using FluentValidation;
using Words.BusinessAccess.Dtos;
using Words.DataAccess.Helpers;

namespace Words.BusinessAccess.ModelValidators;

public class WordCollectionCreateDtoValidator : AbstractValidator<WordCollectionCreateDto>
{
    public WordCollectionCreateDtoValidator()
    {
        const int nameMinLength = WordCollectionConstraints.NameMinLength;
        const int nameMaxLength = WordCollectionConstraints.NameMaxLength;
        RuleFor(x => x.Name)
            .MinimumLength(nameMinLength)
            .MaximumLength(nameMaxLength)
            .WithMessage($"Word collection name length must be between {nameMinLength} and {nameMaxLength}");
        RuleFor(x => x.EnglishLevel)
            .IsInEnum()
            .WithMessage("Invalid english level");
        RuleForEach(x => x.Words)
            .SetValidator(new WordCreateDtoValidator());
    }
}