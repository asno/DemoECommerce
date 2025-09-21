using Microsoft.AspNetCore.Http;

namespace ECommerce.SharedLibrary.Middleware
{
    public class ApiGatewayEnforcement (RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var signedHeader = context.Response.Headers["api-gateway"];

            if (signedHeader.FirstOrDefault() is null)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("Service unavailable.");
                return;
            }
            else
            {
                await next(context);
            }
        }
    }
}
