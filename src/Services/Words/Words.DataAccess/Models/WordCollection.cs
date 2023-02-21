﻿using Shared.Enums;

namespace Words.DataAccess.Models;

public class WordCollection
{
    public int Id { get; set; }
    public string Name { get; set; }
    public EnglishLevel EnglishLevel { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
    public int UserId { get; set; }
    public User User { get; set; }

    public List<Word> Words { get; set; }
    public List<WordCollectionRating> Ratings { get; set; }
}