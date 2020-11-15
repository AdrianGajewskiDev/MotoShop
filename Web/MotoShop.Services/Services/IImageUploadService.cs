using Microsoft.AspNetCore.Http;
using MotoShop.Services.HelperModels;
using System;
using System.Threading.Tasks;

namespace MotoShop.Services.Services
{
    public interface IImageUploadService
    {
        Task<ImageUploadResult> UploadImageAsync(IFormFile file, Action<string> onUploadFinished = null);
    }
}
