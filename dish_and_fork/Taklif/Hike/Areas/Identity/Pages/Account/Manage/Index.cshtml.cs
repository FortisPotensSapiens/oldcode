using System.Threading.Tasks;
using Hike.Entities;
using Hike.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hike.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<HikeUser> _userManager;
        private readonly SignInManager<HikeUser> _signInManager;
        private readonly IReturnUrlRepository _returnUrlRepository;

        public IndexModel(
            UserManager<HikeUser> userManager,
            SignInManager<HikeUser> signInManager,
            IReturnUrlRepository returnUrlRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _returnUrlRepository = returnUrlRepository;
        }

        [Display(Name = "Email")]
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Укажите псевдоним!")]
            [Display(Name = "Псевдоним/Логин/Ник нейм")]
            [StringLength(100, MinimumLength = 1, ErrorMessage = "Псевдоним должен быть от 1 до 100 символов длинной")]
            [RegularExpression(@"^[^\s]*$", ErrorMessage = "Пробелы запрещены!")]
            public string UserName { get; set; }
        }

        private async Task LoadAsync(HikeUser user)
        {
            var userName = await _userManager.GetEmailAsync(user);
            Username = userName;
            Input = new InputModel
            {
                UserName = user.UserName,
            };
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) && !returnUrl.Contains("/profile"))
                await _returnUrlRepository.SetAsync(returnUrl);
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Не удалось найти пользователя с ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Не найден пользователь с ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }
            user.UserName = Input.UserName;
            user.NormalizedUserName = Input.UserName?.ToUpper();
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                StatusMessage = "Ошибка - При попытке изменить данные пользователя случилась непредвиденная ошибка.";
                return RedirectToPage();
            }
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Ваш профиль был обновлен";
            return RedirectToPage();
        }
    }
}
