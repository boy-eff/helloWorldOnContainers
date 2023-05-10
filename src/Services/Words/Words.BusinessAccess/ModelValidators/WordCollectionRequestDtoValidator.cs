using FluentValidation;
using Words.BusinessAccess.Dtos;
using Words.BusinessAccess.Dtos.WordCollection;
using Words.DataAccess.Helpers;

namespace Words.BusinessAccess.ModelValidators;

public class WordCollectionRequestDtoValidator : AbstractValidator<WordCollectionRequestDto>
{
    public WordCollectionRequestDtoValidator()
    {
        const int nameMinLength = WordCollectionConstraints.NameMinLength;
        const int nameMaxLength = WordCollectionConstraints.NameMaxLength;
        const int wordsMinCount = WordCollectionConstraints.WordsMinCount;
        RuleFor(x => x.Name)
            .MinimumLength(nameMinLength)
            .MaximumLength(nameMaxLength)
            .WithMessage($"Word collection name length must be between {nameMinLength} and {nameMaxLength}");
        RuleFor(x => x.EnglishLevel)
            .IsInEnum()
            .WithMessage("Invalid english level");
        RuleFor(x => x.Words)
            .Must(x => x.Count() >= 3)
            .WithMessage($"Word collection must have at least {wordsMinCount} words");
        RuleForEach(x => x.Words)
            .SetValidator(new WordCreateDtoValidator());
    }
}