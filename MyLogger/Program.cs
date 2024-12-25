using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml;

public interface ILoggerRepository
{
    void Log(string message);
}

public class TextFileLogger : ILoggerRepository
{
    private readonly string _filePath;

    public TextFileLogger(string filePath)
    {
        _filePath = filePath;
    }

    public void Log(string message)
    {
        File.AppendAllText(_filePath, message + Environment.NewLine);
    }
}

public class JsonLogger : ILoggerRepository
{
    private readonly string _filePath;

    public JsonLogger(string filePath)
    {
        _filePath = filePath;
    }

    public void Log(string message)
    {
        var existingLog = File.Exists(_filePath) ?
            JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(_filePath)) :
            new List<string>();

        existingLog.Add(message);
        File.WriteAllText(_filePath, JsonConvert.SerializeObject(existingLog, Formatting.Indented));
    }
}

public class MyLogger
{
    private readonly ILoggerRepository _loggerRepository;

    public MyLogger(ILoggerRepository loggerRepository)
    {
        _loggerRepository = loggerRepository;
    }

    public void Log(string message)
    {
        _loggerRepository.Log(message);
    }
}

// Пример использования
class Program
{
    static void Main()
    {
        MyLogger logger = new MyLogger(new JsonLogger("log.json"));
        logger.Log("This is a log message.");
    }
}
