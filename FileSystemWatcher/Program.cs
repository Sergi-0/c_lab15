using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;

public class FileSystemWatcher
{
    private readonly string _directoryPath;
    private readonly Timer _timer;
    private List<string> _previousFiles;

    public event Action<string> FileChanged;

    public FileSystemWatcher(string directoryPath, double interval)
    {
        _directoryPath = directoryPath;
        _timer = new Timer(interval);
        _timer.Elapsed += CheckForChanges;
        _previousFiles = Directory.GetFiles(_directoryPath).ToList();

        _timer.Start();
    }

    private void CheckForChanges(object sender, ElapsedEventArgs e)
    {
        var currentFiles = Directory.GetFiles(_directoryPath);
        var addedFiles = currentFiles.Except(_previousFiles).ToList();
        var deletedFiles = _previousFiles.Except(currentFiles).ToList();

        foreach (var file in addedFiles)
        {
            FileChanged?.Invoke($"File added: {file}");
        }

        foreach (var file in deletedFiles)
        {
            FileChanged?.Invoke($"File removed: {file}");
        }

        _previousFiles = currentFiles.ToList();
    }

    public void Stop()
    {
        _timer.Stop();
    }
}

// Пример использования
class Program
{
    static void Main()
    {
        var watcher = new FileSystemWatcher("C:\\MyDir", 1000);
        watcher.FileChanged += message => Console.WriteLine(message);

        Console.WriteLine("Press Enter to exit.");
        Console.ReadLine();

        watcher.Stop();
    }
}