using Filyzer.Attributes;
using Filyzer.Domain.Entities;

namespace Filyzer.Middleware
{
    public class RoleAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public RoleAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint == null)
            {
                await _next(context);
                return;
            }

            var roleAttributes = endpoint.Metadata
                .OfType<RequireRoleAttribute>()
                .ToList();

            if (roleAttributes.Count == 0)
            {
                await _next(context);
                return;
            }

            if (!context.Items.ContainsKey("User"))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(new { message = "Unauthorized" });
                return;
            }

            var user = context.Items["User"] as User;
            var hasRequiredRole = roleAttributes
                .Any(attr => attr.RequiredRoles.Contains(user.Role));

            if (!hasRequiredRole)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsJsonAsync(new { message = "Forbidden" });
                return;
            }

            await _next(context);
        }
    }
}
