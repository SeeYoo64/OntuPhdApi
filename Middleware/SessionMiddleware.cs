using OntuPhdApi.Services.Authorization;

namespace OntuPhdApi.Middleware
{
    public class SessionMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ISessionService sessionService)
        {
            var sessionToken = context.Request.Cookies["auth.session-token"];
            if (!string.IsNullOrEmpty(sessionToken))
            {
                var user = await sessionService.ValidateSessionAsync(sessionToken);
                if (user != null)
                {
                    context.Items["User"] = user;
                    context.Items["UserId"] = user.Id;
                }
            }

            await _next(context);
        }
    }


    public static class SessionMiddlewareExtensions
    {
        public static IApplicationBuilder UseSessionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SessionMiddleware>();
        }
    }


}
