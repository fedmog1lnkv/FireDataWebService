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

        public List<WeatherDataModel> FilterWeatherByDateRange(DateTime? startDate, DateTime? endDate)
        {
            var filteredWeather = _weather
                .Where(w => w.Date >= startDate && w.Date <= endDate)
                .GroupBy(w => w.WeatherStationId)
                .Select(group =>
                    {
                        // Вычисляем средние значения для каждой метеостанции
                        double avgT = group.Average(w => w.T ?? 0);
                        double avgFf = group.Average(w => w.Ff ?? 0);
                        double avgP = group.Average(w => w.P ?? 0);
                        double avgU = group.Average(w => w.U ?? 0);
                        double avgV = group.Average(w => w.V ?? 0);
                        double avgVV = group.Average(w => w.VV ?? 0);
                        double avgTd = group.Average(w => w.Td ?? 0);
                        double avgRRR = group.Average(w => w.RRR ?? 0);

                        // Получаем уникальные координаты (широта и долгота) для метеостанции
                        var firstWeather = group.FirstOrDefault();
                        double? lat = firstWeather?.Lat;
                        double? lon = firstWeather?.Lon;

                        // Получаем количество встречающихся типов погодных явлений и сортируем по количеству
                        var weatherTypesCount = group.Where(w => !string.IsNullOrEmpty(w.WWType))
                            .GroupBy(w => w.WWType)
                            .OrderByDescending(g => g.Count())
                            .Select(g => g.Key);

                        // Создаем строку из типов погодных явлений, отсортированных по количеству
                        string weatherTypesString = string.Join(", ", weatherTypesCount);

                        return new WeatherDataModel
                        {
                            WeatherStationId = group.Key,
                            Date = startDate.Value,
                            T = avgT,
                            Ff = avgFf,
                            P = avgP,
                            U = avgU,
                            V = avgV,
                            VV = avgVV,
                            Td = avgTd,
                            RRR = avgRRR,
                            Lat = lat,
                            Lon = lon,
                            WWType = weatherTypesString
                        };
                    })
                .ToList();

            return filteredWeather;
        }
    }
}
