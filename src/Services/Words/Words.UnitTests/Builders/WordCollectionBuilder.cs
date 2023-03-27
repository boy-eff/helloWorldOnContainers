using Mapster;
using Microsoft.IdentityModel.Tokens;
using Shared.Enums;
using Words.BusinessAccess.Dtos;
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
            .WithName()
            .WithWords()
            .WithEnglishLevel();
    }

    public WordCollectionBuilder WithId(int id = 1)
    {
        _wordCollection.Id = id;
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

    public WordCollectionDto BuildAsDto()
    {
        return _wordCollection.Adapt<WordCollectionDto>();
    }

    public WordCollectionCreateDto BuildAsCreateDto()
    {
        return _wordCollection.Adapt<WordCollectionCreateDto>();
    }
}