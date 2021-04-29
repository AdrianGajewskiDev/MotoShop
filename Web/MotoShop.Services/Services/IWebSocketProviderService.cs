using MotoShop.Services.HelperModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MotoShop.Services.Services
{
    public interface IWebSocketProviderService
    {
        Task AddConnectionAsync(string connectionID, string data);
        void RemoveConnection(string connectionID);
        Task UpdateConnectionIDAsync(string data, string newConnectionID);
        Task<bool> HasActivConnection(string userID);
    }
}
