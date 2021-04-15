using Microsoft.AspNetCore.Http;
using MotoShop.Data.Database_Context;
using MotoShop.Data.Models.Store;
using MotoShop.Services.HelperModels;
using MotoShop.Services.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.Services.Implementation
{
    public class ImageUploader : IImageUploadService
    {
        public string SavePath => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "resources", "images");
        private readonly ApplicationDatabaseContext _dbContext;

        public ImageUploader(ApplicationDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void DeleteImage(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
                return;

            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), imageName.Replace(@"/",@"\"));

            File.Delete(fullPath);
        }

        /// <summary>
        /// Uploads image to the database
        /// </summary>
        /// <param name="image">File to upload</param>
        /// <param name="onUploadFinished">Gets called once the image uploading is done</param>
        /// <returns></returns>
        public async Task<ImageUploadResult> UploadImageAsync(IFormFile image, Action onUploadFinished = null)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            string uniqueFileName = GenerateUniqueName(image);
            string savePath = SavePath;
            string fullPath = Path.Combine(savePath, uniqueFileName.ToString());
            string dbPath = Path.Combine("wwwroot", "resources", "images", uniqueFileName.ToString()).Replace(@"\", @"/");
            await SaveImage(image, fullPath);

            var result = new ImageUploadResult
            {
                Path = dbPath
            };

            if(result.Success)
                onUploadFinished?.Invoke();

            return result;

        }

        public async Task<MultipleImageUploadResult> UploadMultipleImagesAsync(IEnumerable<IFormFile> formDataImages, int advertisementID)
        {
            var images = new List<Image>();
            var result = new MultipleImageUploadResult();
            Dictionary<IFormFile, Image> pairValues = new Dictionary<IFormFile, Image>();
            List<Task> saveTasks = new List<Task>();

            foreach (var image in formDataImages)
            {

                var newImage = new Image
                {
                    Deleted = false,
                    FilePath = GenerateUniqueName(image).Trim()
                };

                images.Add(newImage);

                pairValues.Add(image, newImage);
            }

            _dbContext.AddRange(images);

            if(_dbContext.SaveChanges() > 0)
            {
                result.Success = true;
                result.Paths = images.Select(x => x.FilePath);

                foreach (var image in formDataImages)
                {
                    saveTasks.Add(SaveImage(image,Path.Combine(SavePath, pairValues[image].FilePath)));
                }

                await Task.WhenAll(saveTasks);

                return result;
            }

            result.Success = false;

            return result;
        }

        public string GenerateUniqueName(IFormFile formFile)
        {
            return new StringBuilder().Append(Path.GetRandomFileName()).Append(ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Trim('"')).ToString();
        }

        private async Task SaveImage(IFormFile image, string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                await image.CopyToAsync(fs);
            }
        }
    }
}
