using MotoShop.Data.Models.Store;
using System.Collections.Generic;

namespace MotoShop.Services.Services
{
    public interface IAdvertisementService
    {
        Advertisement GetAdvertisementById(int id, bool includeAuthorAndItem = true);
        IEnumerable<Advertisement> GetAllAdvertisementsByAuthorId(string authorID);
        IEnumerable<Advertisement> GetAll();
    }
}
