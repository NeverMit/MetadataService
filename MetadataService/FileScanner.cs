using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace MetadataService;

public class FileScanner
{
    //путь к папке сканирования
    private readonly string? _directoryPath;
    //маска файла
    private readonly string? _searchPattern;
    //интервал опроса каталога
    private readonly TimeSpan _interval;

    private List<FileData> _files;
    
    // Конструктор
    public FileScanner(IConfiguration configuration)
    {
        _directoryPath = configuration.GetValue<string>("FileScanner:Path");
        _searchPattern = configuration.GetValue<string>("FileScanner:SearchPattern");
        _interval = TimeSpan.FromSeconds(configuration.GetValue<int>("FileScanner:Interval"));
        _files = new List<FileData>();
    }
    
    // Сканирование файлов
    public async Task ScanFilesAsync()
    {
        while (true)
        {
            if (_directoryPath != null && _searchPattern!=null)
            {
                try
                {
                    var files = Directory.GetFiles(_directoryPath, _searchPattern);
                    Console.WriteLine($"Найдено {files.Length} файлов");
                    foreach (var file in files)
                    {
                        var fileData = await ReadFileDataAsync(file);
                        SeekFileChanges(fileData);
                        Console.WriteLine(
                            $"File:{fileData.FileName}," +
                            $"Content:{fileData.FileHash}"
                            );
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
            await Task.Delay(_interval);
        }
    }

    private void SeekFileChanges(FileData fileData)
    {
        var f = _files.Find(x => 
            x.FileName.Equals(
                fileData.FileName,
                StringComparison.CurrentCultureIgnoreCase)
        );
        if(f==null)
            _files.Add(fileData);
        else
        {
            if (f.FileHash != fileData.FileHash)
            {
                Console.WriteLine(f.FileName+" изменен");
                f.FileHash = fileData.FileHash;
            }
        }
    }
    //чтение содержимого файла
    private static async Task<FileData> ReadFileDataAsync(string filePath)
    {
            await using var fileStream = new FileStream(
                filePath,
                FileMode.Open,
                FileAccess.Read
            );
            using var streamReader = new StreamReader(fileStream);
            var fileContent = await streamReader.ReadToEndAsync();
            var fileName = Path.GetFileName(filePath);
            var fileData = new FileData
            {
                FileName = fileName,
                FileHash = fileContent.GetHashCode()
            };
            return fileData;
    }
}