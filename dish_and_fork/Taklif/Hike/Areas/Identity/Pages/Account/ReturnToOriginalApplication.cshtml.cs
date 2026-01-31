using System.Threading.Tasks;
using Hike.Repositories;
using Hike.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hike.Areas.Identity.Pages.Account
{
    public class ReturnToOriginalApplicationModel : PageModel
    {
        private readonly IReturnUrlRepository _returnUrlRepository;
        private readonly IRedirectService _redirectService;

        public ReturnToOriginalApplicationModel(IReturnUrlRepository returnUrlRepository, IRedirectService redirectService)
        {
            _returnUrlRepository = returnUrlRepository;
            _redirectService = redirectService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            return LocalRedirect("~/");
        }
    }
}
