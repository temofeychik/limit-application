using LimiterApplication.Helpers;

namespace LimiterApplication.Middlewares
{
    public class ApiTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.Request.Path.Value?.Contains("/api");
            if (endpoint.GetValueOrDefault(false))
            {
                if (!context.Request.Headers.TryGetValue(Constants.TOKEN, out
                var extractedToken))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Token was not provided ");
                    return;
                }
                var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
                var apiKey = appSettings.GetValue<string>(Constants.TOKEN);
                if (!apiKey.Equals(extractedToken))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Unauthorized client");
                    return;
                }
            }

            await _next(context);
        }
    }
}
