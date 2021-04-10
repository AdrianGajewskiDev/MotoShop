using Microsoft.EntityFrameworkCore;
using MotoShop.Data.Database_Context;
using MotoShop.Data.Models.Store;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MotoShop.Services.EntityFramework.CompiledQueries
{
    public class AdvertisementQueries
    {
        public static Func<ApplicationDatabaseContext, int, Advertisement> GetByID =
               EF.CompileQuery((ApplicationDatabaseContext db, int id) =>
                db.Advertisements
                .Include(x => x.Author)
                .Include(x => x.ShopItem)
                .Include(x => x.Images)
                .Single(c => c.ID == id));


        public static Func<ApplicationDatabaseContext, IEnumerable<Advertisement>> GetAllWithAuthorAndShopItem =
            EF.CompileQuery((ApplicationDatabaseContext ctx) =>
            ctx.Advertisements
                .Include(c => c.ShopItem)
                .Include(c => c.Author));

        public static Func<ApplicationDatabaseContext, IEnumerable<Advertisement>> GetAllWithShopItem =
          EF.CompileQuery((ApplicationDatabaseContext ctx) =>
          ctx.Advertisements
              .Include(c => c.ShopItem));

        public static Func<ApplicationDatabaseContext, IEnumerable<Advertisement>> GetAdvertisementsWithCars = (ctx) => GetAllWithShopItem.Invoke(ctx).Where(x => x.ShopItem is Car);

        public static Func<ApplicationDatabaseContext, string, IEnumerable<Advertisement>> GetAllAdvertisementsByAuthorId =
            EF.CompileQuery((ApplicationDatabaseContext ctx, string userID) =>
                    ctx.Advertisements.Where(x => x.AuthorID == userID).Include(x => x.ShopItem));
        
    }
}
