using FireDataWebService.Domain.Models;
using FireDataWebService.Domain.Repositories;
using CsvHelper;
using CsvHelper.Configuration;
using FireDataWebService.Infrastructure.Mappers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FireDataWebService.Infrastructure.Repositories
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly string _csvFilePath;

        public WeatherRepository(string csvFilePath)
        {
            _csvFilePath = csvFilePath;
        }

        public async Task<List<WeatherDataModel>> GetAllWeatherAsync()
        {
            List<WeatherDataModel> weatherDataList = new List<WeatherDataModel>();
            
            List<string> badRecord = new List<string>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                HasHeaderRecord = true,
                BadDataFound = context => badRecord.Add(context.RawRecord),
                MissingFieldFound = null
            };
            

            using (var reader = new StreamReader(_csvFilePath))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<WeatherDataMapper>();
                weatherDataList = csv.GetRecords<WeatherDataModel>().ToList();
            }
            
            return weatherDataList;
        }
        
    }
}
