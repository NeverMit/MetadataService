using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MetadataService
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            //вызываем метод настройкаи хоста приложения
            await FileScannerBuilder.Builder();
        }
    }
}