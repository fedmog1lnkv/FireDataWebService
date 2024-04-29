using FireDataWebService.Domain.Models;
using InteractiveMapWeb.Infrastructure.InMemoryStorage;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireDataWebService.Controllers
{
    [Route("weather")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherInMemoryStorage _weatherStorage;

        public WeatherController(WeatherInMemoryStorage weatherStorage)
        {
            _weatherStorage = weatherStorage;
        }

        [HttpGet("geojson")]
        public async Task<IActionResult> GetWeatherGeoJson(DateTime? requestedDate = null)
        {
            var weather = _weatherStorage.GetAllWeather();
            if (requestedDate != null)
            {
                weather = _weatherStorage.FilterWeatherByDate(requestedDate);
            }
            Console.WriteLine($"Number of weather stations: {weather.Count}");
            var geoJson = SerializeToGeoJson(weather);
            return Ok(geoJson);
        }

        private string SerializeToGeoJson(IEnumerable<WeatherDataModel> weatherData)
        {
            var features = weatherData.Select(data => new
            {
                type = "Feature",
                geometry = new
                {
                    type = "Point",
                    coordinates = new[] { data.Lon, data.Lat }
                },
                properties = new
                {
                    weather_station_id = data.WeatherStationId,
                    date = data.Date.ToString("yyyy-MM-dd"),
                    WW = data.WW,
                    T = data.T,
                    Ff = data.Ff,
                    P = data.P,
                    U = data.U,
                    V = data.V,
                    VV = data.VV,
                    Td = data.Td,
                    RRR = data.RRR,
                    WW_code = data.WWCode,
                    WW_type = data.WWType
                }
            });

            var featureCollection = new
            {
                type = "FeatureCollection",
                features = features
            };

            return JsonConvert.SerializeObject(featureCollection);
        }
    }
}
