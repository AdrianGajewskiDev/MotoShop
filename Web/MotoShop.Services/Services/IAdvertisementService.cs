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
        bool IsOwner(string userID, Advertisement ad) 
        {
            return ad.AuthorID == userID;
        }
        IEnumerable<Advertisement> GetByTitle(string title);
        Task<bool> AddAdvertisementAsync(Advertisement advertisement);
        Task<bool> UpdateAdvertisementAsync(int id, Advertisement newAdvertisement, Advertisement oldAdvertisement);
        void DeleteAdvertisement(int advertisementID);

    }
}
