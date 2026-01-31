namespace Hike.Clients
{
    public class DostavistaResponseBase
    {
        public bool IsSuccessful { get; set; }

        public List<string> Errors { get; set; } = new List<string>();
        public string ParameterErrors { get; set; }
        public List<string> Warnings { get; set; } = new List<string>();
        public dynamic ParameterWarnings { get; set; }
    }
}
