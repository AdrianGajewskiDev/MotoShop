using AutoMapper;
using MotoShop.WebAPI.Models.Requests;
using System.Collections.Generic;
using System.Linq;

namespace MotoShop.WebAPI.Extensions
{
    public static class IMapperExtensions
    {
        public static TResult MapFromCollection<TResult>(this IMapper mapper, IEnumerable<UpdateDataModel> sourceCollection, TResult originalObject) 
            where TResult : new()
        {
            var originalObjectType = originalObject.GetType();
            var originalObjectProperties = originalObjectType.GetProperties();
            var typeInstance = new TResult();
            var properties = typeInstance.GetType().GetProperties();

            foreach (var prop in properties)
            {
                foreach (var data in sourceCollection)
                {
                    if (prop.Name.ToLower().Equals(data.Key))
                    {
                        prop.SetValue(typeInstance, data.Content);

                        break;
                    }
                    else
                    {
                        var dataToCopy = originalObjectProperties.Where(x => x.Name == prop.Name).First();
                        prop.SetValue(typeInstance, dataToCopy.GetValue(originalObject));

                        break;
                    }
                }
            }

            return typeInstance;

        }
    }
}
