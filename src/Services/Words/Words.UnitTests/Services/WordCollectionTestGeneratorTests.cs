using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Exceptions;
using Words.BusinessAccess.Services;
using Words.DataAccess;
using Words.DataAccess.Models;
using Words.UnitTests.Builders;
using Words.UnitTests.Helpers;

namespace Words.UnitTests.Services;

public class WordCollectionTestGeneratorTests
{
    private WordsDbContext _dbContext;
    private Mock<ILogger<WordCollectionTestGenerator>> _loggerMock;
    private WordCollectionTestGenerator _sut;

    [SetUp]
    public void Setup()
    {
        _dbContext = DbContextProvider.GetMemoryContext();
        _loggerMock = new Mock<ILogger<WordCollectionTestGenerator>>();
        _sut = new WordCollectionTestGenerator(_dbContext, _loggerMock.Object);
    }

    [TearDown]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task GenerateTestsFromCollection_WhenCollectionContainsOnlyOneWord_ShouldThrowWrongActionException()
    {
        const int optionsCount = 10;
        var collection = WordCollectionBuilder
            .Default()
            .Simple()
            .Build();

        await _dbContext.Collections.AddAsync(collection);
        await  _dbContext.SaveChangesAsync();


        await _sut.Invoking(x => x.GenerateTestsFromCollection(collection.Id, optionsCount))
            .Should().ThrowAsync<WrongActionException>();
    }
    
    [Test]
    public async Task GenerateTestsFromCollection_WhenAnswerOptionsCountExceedsWordsAmount_ShouldDecreaseOptionsCount()
    {
        const int optionsCount = 10;
        var collection = SeedCollectionWithWords();

        await _dbContext.Collections.AddAsync(collection);
        await  _dbContext.SaveChangesAsync();


        var result = await _sut.GenerateTestsFromCollection(collection.Id, optionsCount);

        foreach (var test in result)
        {
            test.AnswerOptions.Count().Should().Be(collection.Words.Count());
        }
    }
    
    [Test]
    public async Task GenerateTestsFromCollection_ReturnsExpectedNumberOfTests()
    {
        var wordCollection = new WordCollection()
        {
            Id = 1,
            Name = "fruits",
            Words = new List<Word>()
            {
                new Word() { Id = 1, WordCollectionId = 1, Value = "apple", Translations = new List<WordTranslation>() { new WordTranslation() { Id = 1, WordId = 1, Translation = "яблоко" } } },
                new Word() { Id = 2, WordCollectionId = 1, Value = "banana", Translations = new List<WordTranslation>() { new WordTranslation() { Id = 2, WordId = 2, Translation = "банан" } } },
                new Word() { Id = 3, WordCollectionId = 1, Value = "pear", Translations = new List<WordTranslation>() { new WordTranslation() { Id = 3, WordId = 3, Translation = "груша" } } },
            }
        };
        
        _dbContext.Collections.Add(wordCollection);
        await _dbContext.SaveChangesAsync();

        const int optionsCount = 3;
        
        var tests = await _sut.GenerateTestsFromCollection(wordCollection.Id, optionsCount);

        tests.Count.Should().Be(wordCollection.Words.Count());
    }
    
    [Test]
    public async Task GenerateTestsFromCollection_WhenCalled_EachQuestionShouldContainOnlyOneCorrectAnswer()
    {
        var wordCollection = SeedCollectionWithWords();
        
        _dbContext.Collections.Add(wordCollection);
        await _dbContext.SaveChangesAsync();

        const int optionsCount = 3;
        
        var tests = await _sut.GenerateTestsFromCollection(wordCollection.Id, optionsCount);

        foreach (var test in tests)
        {
            var answerOptionsFromTest = test.AnswerOptions.Select(x => x.Value);
            var translationsFromWord = wordCollection.Words
                .FirstOrDefault(x => x.Id == test.Word.Id)
                .Translations.Select(x => x.Translation);

            answerOptionsFromTest.Intersect(translationsFromWord).Count().Should().Be(1);
        }
    }

    private static WordCollection SeedCollectionWithWords() => new WordCollection
    {
        Id = 1,
        Name = "fruits",
        Words = new List<Word>
        {
            new()
            {
                Id = 1,
                WordCollectionId = 1,
                Value = "apple",
                Translations = new List<WordTranslation>()
                {
                    new() { Id = 1, WordId = 1, Translation = "яблоко" },
                    new() { Id = 2, WordId = 2, Translation = "yabloko" }
                }
            },
            new()
            {
                Id = 2,
                WordCollectionId = 1,
                Value = "banana",
                Translations = new List<WordTranslation>()
                {
                    new() { Id = 3, WordId = 2, Translation = "банан" },
                    new() { Id = 4, WordId = 2, Translation = "banana" },
                }
            },
            new()
            {
                Id = 3,
                WordCollectionId = 1,
                Value = "pear",
                Translations = new List<WordTranslation>()
                {
                    new() { Id = 5, WordId = 3, Translation = "груша" },
                    new() { Id = 6, WordId = 3, Translation = "grusha" },
                }
            },
        }
    };
}