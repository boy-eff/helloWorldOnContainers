using Microsoft.EntityFrameworkCore;
using Words.BusinessAccess.Contracts;
using Words.BusinessAccess.Extensions;
using Words.BusinessAccess.Models;
using Words.DataAccess;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.Services;

public class WordCollectionTestGenerator : IWordCollectionTestGenerator
{
    private readonly Random _random = new Random();
    private readonly WordsDbContext _dbContext;

    public WordCollectionTestGenerator(WordsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ICollection<WordCollectionTest>> GenerateTestsFromCollection(int wordCollectionId, int answerOptionsCount)
    {
        var wordCollection = await _dbContext.Collections
            .AsNoTracking()
            .Include(x => x.Words)
            .ThenInclude(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == wordCollectionId);
        
        var tests = InitializeTests(wordCollection);

        var possibleTranslations = GeneratePossibleTranslations(wordCollection);

        foreach (var test in tests)
        {
            var usedTranslations = new List<int>();
            for (var i = 0; i < answerOptionsCount - 1; i++)
            {
                int translationIndex;
                do
                {
                    translationIndex = _random.Next(possibleTranslations.Count);
                } while (usedTranslations.Contains(translationIndex) ||
                         test.Word.Translations.Select(x => x.Translation).Contains(possibleTranslations[translationIndex]));
                
                usedTranslations.Add(translationIndex);
                var answerOption = new AnswerOption() { Value = possibleTranslations[translationIndex] };
                test.AnswerOptions.Add(answerOption);
            }

            test.AnswerOptions.Shuffle();
        }

        return tests;
    }

    private List<string> GeneratePossibleTranslations(WordCollection wordCollection) => wordCollection.Words
        .SelectMany(x => x.Translations)
        .Select(x => x.Translation)
        .ToList();

    private ICollection<WordCollectionTest> InitializeTests(WordCollection wordCollection) => wordCollection.Words
        .OrderBy(word => _random.Next())
        .Select(word =>
        {
            var randomIndex = _random.Next(word.Translations.Count);
            return new WordCollectionTest()
            {
                Word = word,
                AnswerOptions = new List<AnswerOption>()
                {
                    new AnswerOption() { Value = word.Translations[randomIndex].Translation, IsCorrect = true }
                }
            };
        }).ToList();
}