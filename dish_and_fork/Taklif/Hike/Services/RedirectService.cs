using System.Text.RegularExpressions;

namespace Hike.Services
{
    public class RedirectService : IRedirectService
    {
        public string ExtractRedirectUriFromReturnUrl(string url)
        {
            var result = "";
            var decodedUrl = System.Net.WebUtility.HtmlDecode(url);
            var results = Regex.Split(decodedUrl, "redirect_uri=");
            if (results.Length < 2)
                return "";
            result = results[1];
             return result.Replace("%3A", ":").Replace("%2F", "/");
            //result = Split(result, "signin-oidc");
            //result = Split(result, "scope");
            //result = Split(result, "response_type");
            //result = Split(result, "login-callbac");
            //return result.Replace("%3A", ":").Replace("%2F", "/").Replace("&", "");
        }

        private string Split(string url, string key)
        {
            var results = Regex.Split(url, key);
            return results[0];
        }
    }
}
