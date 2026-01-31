using System.Text;
using System.Threading.Tasks;
using Hike.Ef;
using Hike.Entities;
using Hike.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace Hike.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<HikeUser> _userManager;
        private readonly IReturnUrlRepository _returnUrlRepository;
        private readonly HikeDbContext _db;

        public ConfirmEmailModel(UserManager<HikeUser> userManager, IReturnUrlRepository returnUrlRepository, HikeDbContext db)
        {
            _userManager = userManager;
            _returnUrlRepository = returnUrlRepository;
            _db = db;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string code, string returnUrl = null)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) && !returnUrl.Contains("/profile"))
                await _returnUrlRepository.SetAsync(returnUrl);
            if (userId == null || code == null)
            {
                return RedirectToPage("/Register");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Не найден пользователь с ID '{userId}'.");
            }
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            StatusMessage = result.Succeeded ? "Ваш email успешно подтвержден." : "Ошибка при подтверждении почты." + string.Join(",", result.Errors?.Select(x => $"({x.Code}:{x.Description}") ?? new List<string>());
            return Page();
        }
    }
}
