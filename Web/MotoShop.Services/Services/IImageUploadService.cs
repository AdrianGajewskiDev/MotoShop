using Microsoft.AspNetCore.Http;
using MotoShop.Services.HelperModels;
using System;
using System.Threading.Tasks;

namespace MotoShop.Services.Services
{
    public interface IImageUploadService
    {
        string SavePath { get;}
        Task<ImageUploadResult> UploadImageAsync(IFormFile file, Action onUploadFinished = null);
        void DeleteImage(string imageName);
    }
}
