using System.Threading.Tasks;
using Hike.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hike.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordConfirmation : PageModel
    {
        private readonly IReturnUrlRepository _returnUrlRepository;

        public ForgotPasswordConfirmation(IReturnUrlRepository returnUrlRepository)
        {
            _returnUrlRepository = returnUrlRepository;
        }
        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) && !returnUrl.Contains("/profile"))
                await _returnUrlRepository.SetAsync(returnUrl);
        }
    }
}
