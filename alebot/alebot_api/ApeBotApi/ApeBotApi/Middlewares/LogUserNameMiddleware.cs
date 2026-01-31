using Serilog.Context;
using System.Security.Claims;

namespace AleBotApi.Middlewares
{
    public class LogUserNameMiddleware
    {
        private readonly RequestDelegate next;

        public LogUserNameMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public Task Invoke(HttpContext context)
        {
            LogContext.PushProperty("UserName", context.User?.FindFirstValue(ClaimTypes.Name));
            LogContext.PushProperty("PreferredUserName", context.User?.FindFirstValue("preferred_username"));
            LogContext.PushProperty("UserId", context.User?.FindFirstValue(ClaimTypes.NameIdentifier));
            return next(context);
        }
    }
}
