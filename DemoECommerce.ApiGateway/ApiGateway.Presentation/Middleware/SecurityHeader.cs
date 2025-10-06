namespace ApiGateway.Presentation.Middleware
{
    public class SecurityHeader(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.Headers["api-gateway"] = "Signed";
            await next(context);
        }
    }
}
