using System.Collections.Generic;

namespace MotoShop.WebAPI.Models.Response
{
    public class PaginatedResponse<T> 
    {
        public int TotalPages { get; set; }
        public T Content { get; set; }
    }
}
