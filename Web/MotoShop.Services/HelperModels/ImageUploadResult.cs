namespace MotoShop.Services.HelperModels
{
    public struct ImageUploadResult
    {
        public string Path { get; set; }
        public bool Success { get => !string.IsNullOrEmpty(Path);}
    }
}
