using MotoShop.Data.Models.Constants;
using MotoShop.Data.Models.Store;
using MotoShop.Services.HelperModels;
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
        IEnumerable<Advertisement> GetByTitle(string title, ICollection<Advertisement> all);
        Task<bool> AddAdvertisementAsync(Advertisement advertisement);
        Task<bool> UpdateAdvertisementAsync(int id, Advertisement newAdvertisement, Advertisement oldAdvertisement);
        void DeleteAdvertisement(int advertisementID);
        void AddImage(int adID, string path);
        TopThreeAdvertisementsResult GetTopThree();
        string GetAdvertisementTitle(int adID);
        public T GetAdvertisementData<T>(int adID, Func<Advertisement, T> dataToSelect);

    }
}
