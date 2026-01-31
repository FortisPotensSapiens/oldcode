using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Hike.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using Serilog.Context;

namespace Hike
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

    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        public RequestResponseLoggingMiddleware(RequestDelegate next,
            ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory
                .CreateLogger<RequestResponseLoggingMiddleware>();
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        public async Task Invoke(HttpContext context)
        {
            await LogRequest(context);
            await _next(context);
            await LogResponse(context);
        }

        private async Task LogRequest(HttpContext context)
        {
         //   var text = await ReadRequest(context);
            _logger.LogInformation($@">>>Http Request {context.TraceIdentifier} Information:{Environment.NewLine}
                                   Schema:{context.Request.Scheme} 
                                   Host: {context.Request.Host} 
                                   Path: {context.Request.Path} 
                                   QueryString: {context.Request.QueryString}
                                    ContentType:{context.Request.ContentType}
                                   Request Headers: {context.Request.Headers?.ToJson()} 
                                   Request Cookies {context.Request.Cookies?.ToJson()} 
                                   Request Body: hiden");
        }

        private async Task<string> ReadRequest(HttpContext context)
        {
            if (!context.Request.HasJsonContentType())
                return string.Empty;
            context.Request.EnableBuffering();
            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);
            var text = ReadStreamInChunks(requestStream);
            text = TrimLength(text);
            context.Request.Body.Position = 0;
            return text;
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 40096;
            stream.Seek(0, SeekOrigin.Begin);
            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);
            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;
            do
            {
                readChunkLength = reader.ReadBlock(readChunk,
                    0,
                    readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);
            return textWriter.ToString();
        }

        private async Task LogResponse(HttpContext context)
        {
           // var text = await ReadResponse(context);
            _logger.LogInformation($@"<<<Http Response {context.TraceIdentifier} Information:{Environment.NewLine} 
                                   StatusCode:{context.Response.StatusCode} 
                                   HasStarted: {context.Response.HasStarted}
                                    ContentType:{context.Response.ContentType}
                                   Response Headers: {context.Response.Headers?.ToJson()} 
                                   Response Cookies {context.Response.Cookies?.ToJson()}
                                   Response Body: hiden");
        }

        private async Task<string> ReadResponse(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            await using var responseBody = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;
            await _next(context);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var text = context.Response?.ContentType?.GetNormalizedName().Contains("JSON") == true ? await new StreamReader(context.Response.Body).ReadToEndAsync() : string.Empty;
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            text = TrimLength(text);
            await responseBody.CopyToAsync(originalBodyStream);
            return text;
        }

        private static string TrimLength(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
                return new string(text.Take(2000).ToArray());
            return text;
        }
    }
}
