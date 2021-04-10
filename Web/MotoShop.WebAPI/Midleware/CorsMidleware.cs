
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MotoShop.WebAPI.Midleware
{
    public class CorsMidleware
    {
        private readonly RequestDelegate _next;

        public CorsMidleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            context.Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:4200");
            context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, X-CSRF-Token, X-Requested-With, Accept, Accept-Version, Content-Length, Content-MD5, Date, X-Api-Version, X-File-Name");
            context.Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET,PUT,PATCH,DELETE,OPTIONS");

            return _next(context);
        }
    }
}
