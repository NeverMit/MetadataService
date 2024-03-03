using Microsoft.Extensions.Configuration;

namespace MetadataService
{
    public class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@"appsettings.json")
                .Build();

            var fileScanner = new FileScanner(configuration);
            fileScanner.ScanFilesAsync().Wait();
        }
    }
}