using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MetadataService;

public class FileScannerBuilder
{
    public static async Task Builder()
    {
        //Настройка хоста приложения
        var builder = new HostBuilder()
            .ConfigureAppConfiguration // настройка конфигурации приложения
            (
                (hostContext, config) =>
                {
                    config.SetBasePath
                    (
                        Directory.GetCurrentDirectory()
                    );
                    config.AddJsonFile
                    (
                        @"appsettings.json",
                        optional: true,
                        reloadOnChange: true
                    );
                }
            )
            .ConfigureServices // подключаем зависимость между FileScanner и FileScannerOptions
            (
                (hostContext, services) =>
                {
                    services.Configure<FileScannerOptions>
                    (
                        hostContext.Configuration.GetSection("FileScanner")
                    );
                    services.AddHostedService<FileScanner>();
                }
            );
        //создаем экземпляр хоста приложения
        await builder.Build().RunAsync();
    }
}