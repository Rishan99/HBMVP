using HouseBrokerMVP.Business.Exceptions;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Net;


namespace HouseBrokerMVP.API.ExcepitonHandler
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var response = context.Response;

            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                // handle as per exception type and log info, for now i am capturing all type and returing message only
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                await response.WriteAsync(error.Message);
            }
        }
    }
}
