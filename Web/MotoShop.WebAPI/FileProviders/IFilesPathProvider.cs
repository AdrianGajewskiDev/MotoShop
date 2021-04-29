namespace MotoShop.WebAPI.FileProviders
{
    public interface IFilesPathProvider
    {
        string PathToSave { get; }
        string RequestPath { get; }
    }
}
