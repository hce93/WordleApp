namespace WordleApp;
using System.IO;
using System.Runtime.InteropServices.Marshalling;

public static class WordGenerator
{
    public static async Task<string> Generate()
    {
        // using var stream = await FileSystem.OpenAppPackageFileAsync("ValidWords.txt");
        // using var reader = new StreamReader(stream);
        
        // var words = await reader.ReadToEndAsync();
        var allWords = await loadWordFile("ValidWords.txt");
        int lineCount = allWords.Length;

        var rand = new Random();
        int wordToFind = rand.Next(0, lineCount); 

        string latestWord = allWords[wordToFind].ToUpper();

        return latestWord;
    }

    public static async Task<bool> checkWord(string word)
    {
        word = word.ToLower();
        var allWords = await loadWordFile("ValidGuesses.txt");
        bool validWord = Array.Exists(allWords, x => x == word);


        return validWord;
    }

    private static async Task<string[]> loadWordFile(string path)
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync(path);
        using var reader = new StreamReader(stream);

        var words = await reader.ReadToEndAsync();
        var allWords = words.Split("\n");

        return allWords;
    }
}
