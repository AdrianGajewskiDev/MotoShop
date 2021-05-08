using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;

namespace MotoShop.WebAPI.Configurations
{
    public class JsonConfiguration
    {
        public static void Configure(MvcNewtonsoftJsonOptions options)
        {
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            options.SerializerSettings.ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new DefaultNamingStrategy()
            };
        }
    }
}

