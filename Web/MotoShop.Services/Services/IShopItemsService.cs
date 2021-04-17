using MotoShop.Data.Models.Constants;
using MotoShop.Data.Models.Store;
using System.Threading.Tasks;

namespace MotoShop.Services.Services
{
    public interface IShopItemsService
    {
        ShopItem GetItemByAdvertisement(int advertisementID);
        void DeleteItem(ShopItem item);
        Task<bool> AddItemAsync(int advertisementID, ShopItem item);
        Task<bool> UpdateItemAsync(int itemID, ShopItem updatedItem);
        ShopItem GetItemByID(int id, ItemType type);
        Car GetCarItem(int id);
        Motocycle GetMotocycleItem(int id);

    }
}
