using Microsoft.EntityFrameworkCore;
using MotoShop.Data.Database_Context;
using MotoShop.Data.Models.Store;
using System;
using System.Linq;

namespace MotoShop.Services.EntityFramework.CompiledQueries
{
    public class ShopItemQueries
    {
        public static T GetByID<T>(ApplicationDatabaseContext context,int id) where T:ShopItem
        {

            if(typeof(T) == typeof(Car))
            {
                Func<ApplicationDatabaseContext, int, Car> d = EF.CompileQuery((ApplicationDatabaseContext db, int id) => db.Cars.Where(x => x.ID == id).FirstOrDefault());

                return (d(context, id)) as T;
            }

            Func<ApplicationDatabaseContext, int, Motocycle> func = EF.CompileQuery((ApplicationDatabaseContext db, int id) => db.Motocycles.Where(x => x.ID == id).FirstOrDefault());

            return func(context, id) as T; 
        }

    }
}
