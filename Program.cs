using FireDataWebService;
using FireDataWebService.AppStart;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace InteractiveMapWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                        webBuilder.UseUrls("http://localhost:5000", "https://localhost:5001"); // Указываем порты для HTTP и HTTPS
                    });
    }
}
