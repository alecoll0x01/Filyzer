using Filyzer.Domain.Interfaces;

namespace Filyzer.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserRepository userRepository)
        {
            if (!context.Request.Headers.TryGetValue("X-API-Key", out var apiKey))
            {
                context.Response.StatusCode = 401;
                return;
            }

            Domain.Entities.User user = await userRepository.GetByApiKeyAsync(apiKey);
            if (user == null)
            {
                context.Response.StatusCode = 401;
                return;
            }

            await _next(context);
        }

    }
}
