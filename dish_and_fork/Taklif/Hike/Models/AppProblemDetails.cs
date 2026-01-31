using Microsoft.AspNetCore.Mvc;

namespace Hike.Models
{
    public class AppProblemDetails : ValidationProblemDetails
    {
        public string TraceId { get; set; }
    }
}
