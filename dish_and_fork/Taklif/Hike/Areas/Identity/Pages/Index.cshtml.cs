using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Hike.Areas.Identity.Pages
{
    public class IndexHomeModel : PageModel
    {
        private readonly ILogger<IndexHomeModel> _logger;

        public IndexHomeModel(ILogger<IndexHomeModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            Redirect("~/Identity/Account/Login");
        }
    }
}
