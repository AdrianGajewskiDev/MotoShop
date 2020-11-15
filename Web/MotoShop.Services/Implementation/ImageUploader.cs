using Microsoft.AspNetCore.Http;
using MotoShop.Services.HelperModels;
using MotoShop.Services.Services;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.Services.Implementation
{
    public class ImageUploader : IImageUploadService
    {
        /// <summary>
        /// Uploads image to the database
        /// </summary>
        /// <param name="image">File to upload</param>
        /// <param name="onUploadFinished">Gets called once the image uploading is done</param>
        /// <returns></returns>
        public async Task<ImageUploadResult> UploadImageAsync(IFormFile image, Action<string> onUploadFinished = null)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            var uniqueFileName = new StringBuilder().Append(Path.GetRandomFileName()).Append(ContentDispositionHeaderValue.Parse(image.ContentDisposition).FileName.Trim('"'));
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "resources", "images");
            string fullPath = Path.Combine(savePath, uniqueFileName.ToString());
            string dbPath = Path.Combine("wwwroot", "resources", "images", uniqueFileName.ToString()).Replace(@"\", @"/");

            using (FileStream fs = new FileStream(fullPath, FileMode.Create))
            {
                await image.CopyToAsync(fs);
            }

            var result = new ImageUploadResult
            {
                Path = dbPath
            };

            if(result.Success)
                onUploadFinished?.Invoke(dbPath);

            return result;

        }
    }
}
