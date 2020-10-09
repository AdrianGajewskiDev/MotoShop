﻿using MotoShop.Data.Models.Store;
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
        void DeleteAdvertisement(int advertisementID);

    }
}
