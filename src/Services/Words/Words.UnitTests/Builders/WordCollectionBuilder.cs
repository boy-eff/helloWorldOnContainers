using Mapster;
using Microsoft.IdentityModel.Tokens;
using Shared.Enums;
using Words.BusinessAccess.Dtos;
using Words.BusinessAccess.Dtos.WordCollection;
using Words.DataAccess.Models;

namespace Words.UnitTests.Builders;

public class WordCollectionBuilder
{
    private WordCollection _wordCollection = new WordCollection();

    public static WordCollectionBuilder Default()
    {
        return new WordCollectionBuilder();
    }

    public WordCollectionBuilder Simple()
    {
        return Default()
            .WithId()
            .WithUserId()
            .WithName()
            .WithWords()
            .WithEnglishLevel()
            .WithDailyViews();
    }

    public WordCollectionBuilder WithId(int id = 1)
    {
        _wordCollection.Id = id;
        return this;
    }
    
    public WordCollectionBuilder WithDailyViews(int views = 0)
    {
        _wordCollection.DailyViews = views;
        return this;
    }

    public WordCollectionBuilder WithUserId(int userId = 1)
    {
        _wordCollection.UserId = userId;
        return this;
    }

    public WordCollectionBuilder WithName(string name = "Test collection")
    {
        _wordCollection.Name = name;
        return this;
    }

    public WordCollectionBuilder WithWords(params Word[] words)
    {
        if (words.IsNullOrEmpty())
        {
            _wordCollection.Words = new List<Word>()
            {
                new Word()
                {
                    Id = 1,
                    Value = "Sample",
                    Translations = new List<WordTranslation>()
                    {
                        new WordTranslation() { Id = 1, Translation = "Пример" }
                    }
                }
            };
        }
        else
        {
            _wordCollection.Words = words;
        }
        return this;
    }

    public WordCollectionBuilder WithEnglishLevel(EnglishLevel englishLevel = EnglishLevel.Advanced)
    {
        _wordCollection.EnglishLevel = englishLevel;
        return this;
    }

    public WordCollection Build()
    {
        return _wordCollection;
    }

    public WordCollectionResponseDto BuildAsResponseDto()
    {
        return _wordCollection.Adapt<WordCollectionResponseDto>();
    }

    public WordCollectionRequestDto BuildAsRequestDto()
    {
        return _wordCollection.Adapt<WordCollectionRequestDto>();
    }
}