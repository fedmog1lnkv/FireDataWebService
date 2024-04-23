using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FireDataWebService.Domain.Models;
using FireDataWebService.Domain.Repositories;
using FireDataWebService.Domain.Mappers;

namespace FireDataWebService.Infrastructure.Repositories
{
    public class FireRepository : IFireRepository
    {
        private readonly string _csvFilePath;

        public FireRepository(string csvFilePath)
        {
            _csvFilePath = csvFilePath;
        }

        public async Task<List<FireDataModel>> GetAllFiresAsync()
        {
            var fireDataList = new List<FireDataModel>();

            var csvLines = File.ReadAllLines(_csvFilePath);

            foreach (var csvLine in csvLines.Skip(1))
            {
                // Преобразуем строку CSV в объект FireDataModel и добавим его в список
                try
                {
                    fireDataList.Add(FireDataMapper.FromCsv(csvLine));
                }
                catch (Exception e)
                {
                    continue;
                }

            }

            return fireDataList;
        }
        
        public async Task<FireDataModel> GetFireByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
