using FireDataWebService.Domain.Models;
using FireDataWebService.Domain.Repositories;

namespace InteractiveMapWeb.Infrastructure.InMemoryStorage
{
    public class WeatherInMemoryStorage
    {
        private readonly List<WeatherDataModel> _weather;

        public WeatherInMemoryStorage(IWeatherRepository weatherRepository)
        {
            _weather = weatherRepository.GetAllWeatherAsync().Result;
            Console.WriteLine("WeatherInMemoryStorage loaded");
        }

        public List<WeatherDataModel> GetAllWeather()
        {
            return _weather;
        }

        public List<WeatherDataModel> FilterWeatherByDate(DateTime? date)
        {
            var filteredWeather = _weather
                .Where(weather => weather.Date.Date == date.Value.Date)
                .ToList();
            
            return filteredWeather;
        }
    }
}
