using Microsoft.EntityFrameworkCore;
using MotoShop.Data.Database_Context;
using MotoShop.Data.Models.User;
using System;
using System.Collections.Generic;

namespace MotoShop.Services.EntityFramework.CompiledQueries
{
    public static class AdministrationQueries
    {
        public static Func<ApplicationDatabaseContext, IEnumerable<ApplicationUser>> GetAllUsers = EF.CompileQuery((ApplicationDatabaseContext db)
            => db.Users
        );
    }
}
