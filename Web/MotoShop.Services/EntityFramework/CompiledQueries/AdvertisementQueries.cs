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
                .Single(c => c.ID == id));


        public static Func<ApplicationDatabaseContext, IEnumerable<Advertisement>> GetAll =
            EF.CompileQuery((ApplicationDatabaseContext ctx) =>
            ctx.Advertisements
                .Include(c => c.ShopItem)
            );

        public static Func<ApplicationDatabaseContext, string, IEnumerable<Advertisement>> GetAllAdvertisementsByAuthorId =
            EF.CompileQuery((ApplicationDatabaseContext ctx, string userID) =>
                    ctx.Advertisements.Where(x => x.AuthorID == userID).Include(x => x.ShopItem));

        public static Func<ApplicationDatabaseContext, string, IEnumerable<Advertisement>> GetByTitle =
            EF.CompileQuery((ApplicationDatabaseContext ctx, string title) =>
                    ctx.Advertisements.Where(x => x.Title.ToLower() == title).
            Include(x => x.ShopItem).
            Include(x => x.Author));
    }
}
