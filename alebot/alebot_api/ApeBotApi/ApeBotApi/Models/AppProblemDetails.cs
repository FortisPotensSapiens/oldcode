using Microsoft.AspNetCore.Mvc;

namespace ApeBotApi.Models
{
    public class AppProblemDetails : ValidationProblemDetails
    {
        public string TraceId { get; set; }
    }
}
