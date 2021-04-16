using Microsoft.AspNetCore.Http;
using MotoShop.Data.Models.Store;
using MotoShop.Services.HelperModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MotoShop.Services.Services
{
    public interface IImageUploadService
    {
        string SavePath { get;}
        Task<ImageUploadResult> UploadImageAsync(IFormFile file, Action onUploadFinished = null);
        Task<MultipleImageUploadResult> UploadMultipleImagesAsync(IEnumerable<IFormFile> formDataImages, int advertisementID);
        void DeleteImage(string imageName);
        string GenerateUniqueName(IFormFile formFile);

        IEnumerable<string> GetImagePathsForItem(int advertisementID);
    }
}
