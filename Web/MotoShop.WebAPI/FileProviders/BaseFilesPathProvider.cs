using System.IO;

namespace MotoShop.WebAPI.FileProviders
{
    public class BaseFilesPathProvider : IFilesPathProvider
    {
        public string PathToSave { get; } = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"resources");

        public string RequestPath { get; } = "/wwwroot/resources";
    }
}
