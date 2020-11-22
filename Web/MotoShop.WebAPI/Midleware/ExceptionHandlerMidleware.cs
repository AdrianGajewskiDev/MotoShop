using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace MotoShop.WebAPI.Midleware
{
    public class ExceptionHandlerMidleware
    {
        private readonly RequestDelegate _requestDelegate;

        public ExceptionHandlerMidleware(RequestDelegate requestDelegate = null)
        {
            _requestDelegate = requestDelegate;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if(_requestDelegate != null)
                    await _requestDelegate(context);
            }
            catch (Exception ex)
            {
                await HandleException(context);
            }
        }

        public async Task HandleException(HttpContext context)
        {
            var handler = context.Features.Get<IExceptionHandlerFeature>();

            if(handler != null && handler.Error != null)
            {
                Log.Information(handler.Error.InnerException, $"A exception was thrown while proceding request from { context.Request.Path}, Message: { handler.Error.Message}");

                var statusCode = HttpStatusCode.InternalServerError;
                switch (handler.Error)
                {
                    case NotImplementedException _:
                        statusCode = HttpStatusCode.NotImplemented;
                        break;
                    case ArgumentNullException _:
                        statusCode = HttpStatusCode.BadRequest;
                        break;
                    case AuthenticationException _:
                        statusCode = HttpStatusCode.Unauthorized;
                        break;

                }
                
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)statusCode;

                var message = JsonConvert.SerializeObject(new { message = handler.Error.Message, Exception = handler.Error.InnerException });

                await context.Response.WriteAsync(message);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response
                    .WriteAsync(JsonConvert.SerializeObject(new { message = "Unhandled internal error" }))
                    .ConfigureAwait(false);
            }
        }
    }
}
