using System.Threading.Tasks;
using Hike.Entities;
using Hike.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hike.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<HikeUser> _userManager;
        private readonly IReturnUrlRepository _returnUrlRepository;

        public RegisterConfirmationModel(UserManager<HikeUser> userManager, IReturnUrlRepository returnUrlRepository)
        {
            _userManager = userManager;
            _returnUrlRepository = returnUrlRepository;
        }

        public string Email { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) && !returnUrl.Contains("/profile"))
                await _returnUrlRepository.SetAsync(returnUrl);
            if (email == null)
            {
                return RedirectToPage("/Index");
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Не найден пользователь с почтой '{email}'.");
            }
            Email = email;
            return Page();
        }
    }
}
