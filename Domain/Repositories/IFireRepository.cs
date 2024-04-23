using System.Collections.Generic;
using System.Threading.Tasks;
using FireDataWebService.Domain.Models;

namespace FireDataWebService.Domain.Repositories
{
    public interface IFireRepository
    {
        Task<List<FireDataModel>> GetAllFiresAsync();
    }
}
