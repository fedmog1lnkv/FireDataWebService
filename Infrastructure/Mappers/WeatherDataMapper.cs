using CsvHelper.Configuration;
using FireDataWebService.Domain.Models;
using System.Globalization;
using System.Text.RegularExpressions;

namespace FireDataWebService.Infrastructure.Mappers
{
    public sealed class WeatherDataMapper : ClassMap<WeatherDataModel>
    {
        public WeatherDataMapper()
        {
            Map(m => m.WeatherStationId).Name("weather_station_id");
            Map(m => m.Date).Name("date");
            Map(m => m.WW).Name("WW");
            Map(m => m.T).Name("T");
            Map(m => m.Ff).Name("Ff");
            Map(m => m.P).Name("P");
            Map(m => m.U).Name("U");
            Map(m => m.V).Name("VV");
            Map(m => m.VV).Name("Td");
            Map(m => m.RRR).Name("RRR");
            Map(m => m.WWCode).Name("WW_code");
            Map(m => m.Lon).Name("lon");
            Map(m => m.Lat).Name("lat");
            Map(m => m.WWType).Name("WW_type");
        }
    }
}
