using System.Globalization;
using System.Text.Json;

var input = """
    Taggia è un affascinante borgo della Riviera Ligure di Ponente, noto per il suo centro storico ricco di vicoli e architetture medievali.
    È celebre anche per la produzione di un pregiato olio extravergine d’oliva e per le tradizionale festa dei Furgari di San Benedetto".
    """;

if (string.HasValue(input))
{
    if (input.IsJson())
    {
        Console.WriteLine("The input is a valid JSON string.");
    }
    else
    {
        var wordCount = input.WordCount;
        Console.WriteLine($"Word Count: {wordCount}");
    }
}

var person = new Person
{
    FirstName = "Marco ",
    LastName = "Minerva",
    City = "Taggia"
};

Console.ReadLine();

public static class StringExtensions
{
    extension(string)
    {
        public static bool HasValue(string? source)
        {
            return !string.IsNullOrWhiteSpace(source);
        }
    }

    extension(string source)
    { 
        public int WordCount
        {
            get
            { 
                return source.Split([' ', '\t', '\n', '\r'],
                    StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Length;
            }
        }

        public bool IsJson(JsonDocumentOptions options = default)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return false;
            }

            try
            {
                using var document = JsonDocument.Parse(source, options);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }
    }
}

public class Person
{
    public string? FirstName
    {
        get;
        set => field = value?.Trim();
    }    

    public string? LastName { get; set; }
    
    public string? City { get; set; }
}