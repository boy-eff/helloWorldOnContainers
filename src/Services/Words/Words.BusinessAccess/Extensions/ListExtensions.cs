using Words.BusinessAccess.Models;

namespace Words.BusinessAccess.Extensions;

public static class ListExtensions
{
    private static readonly Random _random = new Random();  
    public static void Shuffle<T>(this IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = _random.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }

    public static void FillWithAnswerOptions(this IEnumerable<WordCollectionTest> tests, List<string> possibleTranslations,
        int answerOptionsCount)
    {
        
    }
}