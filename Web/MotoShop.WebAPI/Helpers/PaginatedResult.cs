using MotoShop.WebAPI.Models.Response;
using System.Collections.Generic;
using System.Linq;

namespace MotoShop.WebAPI.Helpers
{
    public static class PaginatedResult
    {
        public static IEnumerable<T> Create<T>(IEnumerable<T> source, int pageSize, int pageNumber) 
        {
            return source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public static PaginatedResponse<T> BuildResponseModel<T>(IEnumerable<T> result, int pageSize)
        {
            var responseModel = new PaginatedResponse<T>
            {
                Content = (T)result,
                TotalPages = result.Count() / pageSize
            };

            return responseModel;
        }
    }
}
