using System.Collections.Generic;

namespace MotoShop.Services.HelperModels
{
    public struct ImageUploadResult
    {
        public string Path { get; set; }
        public bool Success { get => !string.IsNullOrEmpty(Path);}
    }


    public struct MultipleImageUploadResult 
    {
        public bool Success { get; set; }
        public IEnumerable<string> Paths { get; set; }
    }
}
