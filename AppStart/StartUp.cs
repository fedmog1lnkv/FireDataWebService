using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FireDataWebService.Domain.Repositories;
using FireDataWebService.Infrastructure.Repositories;
using InteractiveMapWeb.Infrastructure.InMemoryStorage;

namespace FireDataWebService.AppStart
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddControllers();
            
            services.Configure<StaticFileOptions>(options =>
                {
                    options.ServeUnknownFileTypes = true;
                    options.DefaultContentType = "application/json";
                });

            // Регистрация репозиториев
            services.AddSingleton<IFireRepository>(provider =>
                {
                    // Путь к файлу CSV с данными о пожарах
                    var firesCsvFilePath = Configuration["FiresCsvFilePath"];
    
                    // Создаем экземпляр FireRepository и передаем путь к файлу CSV
                    return new FireRepository(firesCsvFilePath);
                });
            services.AddSingleton<FireInMemoryStorage>();
            
            services.AddSingleton<IWeatherRepository>(provider =>
                {
                    var weatherCsvFilePath = Configuration["WeatherCsvFilePath"];
    
                    return new WeatherRepository(weatherCsvFilePath);
                });
            services.AddSingleton<WeatherInMemoryStorage>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            
            app.UseCors("AllowOrigin");

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                });
        }
    }
}
