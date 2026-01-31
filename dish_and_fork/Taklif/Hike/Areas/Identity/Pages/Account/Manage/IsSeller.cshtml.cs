using System.Threading.Tasks;
using Hike.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hike.Areas.Identity.Pages.Account.Manage
{
    public class IsSellerModel : PageModel
    {
        private readonly UserManager<HikeUser> _userManager;
        private readonly SignInManager<HikeUser> _signInManager;

        public IsSellerModel(UserManager<HikeUser> userManager, SignInManager<HikeUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Не найден пользователь с ID '{_userManager.GetUserId(User)}'.");
            }
            if(await  _userManager.IsInRoleAsync(user, "seller"))
                return RedirectToPage("Index");
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var updateResult = await _userManager.AddToRoleAsync(user, "seller");
            if (!updateResult.Succeeded)
            {
                StatusMessage = "Ошибка - При попытке изменить данные пользователя случилась непредвиденная ошибка.";
                return RedirectToPage();
            }
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Ваш профиль был обновлен. Теперь вы можете пользоватся функционалом для продавца";
            return RedirectToPage("Index");
        }
    }
}
