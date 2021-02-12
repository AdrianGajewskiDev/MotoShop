using MotoShop.Data.Models.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotoShop.Services.Services
{
    public interface IAdvertisementService
    {
        Advertisement GetAdvertisementById(int id, bool includeAuthorAndItem = true);
        IEnumerable<Advertisement> GetAllAdvertisementsByAuthorId(string authorID);
        IEnumerable<Advertisement> GetAll();

        Task<bool> AddAdvertisementAsync(Advertisement advertisement);
        Task<bool> UpdateAdvertisementAsync(int id, string dataType, string content);
        Task<bool> UpdateAdvertisementAsync(int id, Advertisement advertisement);
        void DeleteAdvertisement(int advertisementID);

    }
}
