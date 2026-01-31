using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Hike
{
    public class LoggingHandler : DelegatingHandler
    {
        private readonly ILogger<LoggingHandler> _log;

        public LoggingHandler(ILogger<LoggingHandler> logger) : base()
        {
            _log = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var builder = new StringBuilder();
            builder.AppendLine("Request:");
            builder.AppendLine(request.ToString());
            if (request.Content != null)
            {
                builder.AppendLine(await request.Content.ReadAsStringAsync(cancellationToken));
            }
            builder.AppendLine();
            try
            {
                HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
                builder.AppendLine("Response:");
                builder.AppendLine(response.ToString());
                if (response.Content != null)
                {
                    builder.AppendLine(await response.Content.ReadAsStringAsync(cancellationToken));
                }
                builder.AppendLine();
                return response;
            }
            finally
            {
                _log.LogInformation(builder.ToString());
            }
        }
    }
}