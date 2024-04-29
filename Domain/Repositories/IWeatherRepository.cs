using FireDataWebService.Domain.Models;

namespace FireDataWebService.Domain.Repositories
{
    public interface IWeatherRepository
    {
        Task<List<WeatherDataModel>> GetAllWeatherAsync();
    }
}
