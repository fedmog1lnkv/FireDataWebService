using System.Collections.Generic;
using FireDataWebService.Domain.Models;
using FireDataWebService.Domain.Repositories;

namespace InteractiveMapWeb.Infrastructure.InMemoryStorage
{
    public class FireInMemoryStorage
    {
        private readonly List<FireDataModel> _fires;

        public FireInMemoryStorage(IFireRepository fireRepository)
        {
            _fires = fireRepository.GetAllFiresAsync().Result;
            Console.WriteLine("FireInMemoryStorage loaded");
        }

        public List<FireDataModel> GetAllFires()
        {
            return _fires;
        }
        
        public List<FireDataModel> FilterFiresByDate(DateTime? date)
        {
            List<FireDataModel> filteredFires = new List<FireDataModel>();
            foreach (var fire in _fires)
            {
                if (fire.DtStart <= date && date <= fire.DtEnd)
                {
                    filteredFires.Add(fire);
                }
            }
            return filteredFires;
        }

        public List<FireDataModel> FilterFiresByDateRange(DateTime? startDate, DateTime? endDate)
        {
            var filteredFires = _fires
                .Where(fire => fire.DtStart <= endDate && fire.DtEnd >= startDate)
                .ToList();

            return filteredFires;
        }
    }
}
