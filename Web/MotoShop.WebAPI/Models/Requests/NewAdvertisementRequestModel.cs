using MotoShop.Data.Models.Store;
using System.Diagnostics.CodeAnalysis;

namespace MotoShop.WebAPI.Models.Requests
{
    public class NewAdvertisementRequestModel 
    {
        public string Title { get; set; }
        public string Description { get; set; }


        [AllowNull]
        public Car Car { get; set; }

        [AllowNull]
        public Motocycle Motocycle{ get; set; }
    }

}
