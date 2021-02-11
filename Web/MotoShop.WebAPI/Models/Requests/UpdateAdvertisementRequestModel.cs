using System.Collections.Generic;

namespace MotoShop.WebAPI.Models.Requests
{
    public class UpdateAdvertisementRequestModel
    {
        public UpdateDataModel[] DataModels { get; set; }
    }

    public class UpdateDataModel
    {
       public string Key { get; set; }
       public string Content { get; set; }
    }
}
