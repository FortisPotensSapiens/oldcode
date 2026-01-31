using System.Text.Json;

namespace Hike.Extensions
{
    public static class ExceptionExtensions
    {
        private const int MaxDepth = 200;

        public static IEnumerable<string> ExceptionMessages(this Exception ex, bool withTrace = false)
        {
            var details = new List<string>();
            var exceptions = new List<Exception>();
            Add(ex, details, exceptions, withTrace);
            return details;
        }

        private static void Add(Exception ex, List<string> details, List<Exception> exceptions, bool withTrace)
        {
            if (ex == null)
                return;
            if (details.Count > MaxDepth)
                return;
            if (exceptions.Contains(ex))
                return;
            details.Add($"[Level: {details.Count}] [Type: {ex.GetType().FullName}] [Message: {ex.Message}] [Data: {JsonSerializer.Serialize(ex.Data)}]" + (withTrace ? $" [StackTrace: {ex.StackTrace}]" : string.Empty));
            exceptions.Add(ex);
            if (ex is AggregateException aggregate)
            {
                foreach (var e in aggregate.InnerExceptions)
                {
                    Add(e, details, exceptions, withTrace);
                }
            }
            Add(ex.InnerException, details, exceptions, withTrace);
        }
    }
}