using System;

public class SingleRandomizer
{
    private static readonly Lazy<SingleRandomizer> _instance =
        new Lazy<SingleRandomizer>(() => new SingleRandomizer());

    private readonly Random _random;

    private SingleRandomizer()
    {
        _random = new Random();
    }

    public static SingleRandomizer Instance => _instance.Value;

    public int GetNext()
    {
        return _random.Next();
    }
}

// Пример использования
class Program
{
    static void Main()
    {
        var randomizer = SingleRandomizer.Instance;
        Console.WriteLine(randomizer.GetNext());
    }
}
