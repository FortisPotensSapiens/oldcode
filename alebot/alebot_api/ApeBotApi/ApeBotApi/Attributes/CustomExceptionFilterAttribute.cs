using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ApeBotApi.Extensions;
using ApeBotApi.Models;

namespace ApeBotApi.Attributes
{
    public class CustomExceptionFilterAttribute : Attribute, IExceptionFilter
    {

        private readonly ILogger _logger;
        public CustomExceptionFilterAttribute(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory
                .CreateLogger<CustomExceptionFilterAttribute>();
        }
        public void OnException(ExceptionContext context)
        {
            if (!(context.Exception is { }))
                return;
            _logger.LogError(context.Exception, "Handled Error In Filter {@Error}", context.Exception);
            if (context.Exception is ApplicationException)
            {
                SetErrorResponse(context, new AppErrorModel()
                {
                    Message = context.Exception.Message,
                    Data = context.Exception.Data,
                    ServerTime = DateTime.UtcNow,
                    TraceId = context.HttpContext.TraceIdentifier
                }, 488);
            }
            else if (context.Exception is DbUpdateConcurrencyException)
            {
                SetErrorResponse(context, new AppErrorModel()
                {
                    Message = "Кто-то изменил данные до вас. Поробуйте снова.",
                    Data = context.Exception.Data,
                    ServerTime = DateTime.UtcNow,
                    TraceId = context.HttpContext.TraceIdentifier
                }, 488);
            }
            else
            {
                SetErrorResponse(context, new ErrorModel()
                {
                    Message = GetMessage(context.Exception, 0),
                    ServerTime = DateTime.UtcNow,
                    TraceId = context.HttpContext.TraceIdentifier
                }, 588);
            }
            context.ExceptionHandled = true;
        }

        private void SetErrorResponse(ExceptionContext context, ErrorModel error, int status)
        {
            context.Result = new ContentResult()
            {
                Content = error.ToJson(),
                ContentType = "application/json",
                StatusCode = status
            };
            context.HttpContext.Response.StatusCode = status;
        }

        private string GetMessage(Exception ex, uint level)
        {
            if (level > 1000)
                return string.Empty;
            if (ex == null)
                return string.Empty;
            if (ex is AggregateException aggregate)
                return ex.Message + " -> " + string.Join(" -> ",
                           aggregate.InnerExceptions.Select(x => GetMessage(x, level + 1)));
            return ex.Message + " -> " + GetMessage(ex.InnerException, level + 1);
        }
    }
}
