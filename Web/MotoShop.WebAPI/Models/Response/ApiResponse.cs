using System.Net;

namespace MotoShop.WebAPI.Models.Response
{
    public class ApiResponse<T> 
    {
        public T ResponseContent { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }

    }
}
