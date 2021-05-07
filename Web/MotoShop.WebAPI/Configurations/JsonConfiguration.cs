using Microsoft.AspNetCore.Mvc;
using MotoShop.WebAPI.Configurations.Policy;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MotoShop.WebAPI.Configurations
{
    public class JsonConfiguration
    {
        public static void Configure(JsonOptions options)
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = new JsonCustomNamingPolicy();
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        }
    }
}

