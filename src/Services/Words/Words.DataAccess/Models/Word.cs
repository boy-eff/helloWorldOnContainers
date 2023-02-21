﻿namespace Words.DataAccess.Models;

public class Word
{
    public int Id { get; set; }
    public string Value { get; set; }
    
    public int WordCollectionId { get; set; }
    public WordCollection WordCollection { get; set; }
    public List<WordTranslation> Translations { get; set; }
    public List<UserWord> UserDictionaries { get; set; }
}