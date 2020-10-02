using Microsoft.AspNetCore.Mvc;
using MotoShop.WebAPI.Configurations.Policy;
using System.Text.Json;

namespace MotoShop.WebAPI.Configurations
{
    public class JsonConfiguration
    {
        public static void Configure(JsonOptions options)
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = new JsonCustomNamingPolicy();
        }
    }
}

