using System.Threading.Tasks;
using Hike.Entities;
using Hike.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Hike.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<HikeUser> _userManager;
        private readonly IReturnUrlRepository _returnUrlRepository;
        private readonly SignInManager<HikeUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<HikeUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<HikeUser> userManager,
            IReturnUrlRepository returnUrlRepository)
        {
            _userManager = userManager;
            _returnUrlRepository = returnUrlRepository;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Укажите данные для входа")]
            [Display(Name = "Еmail или Логин")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Укажите пароль")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Запомнить меня?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) && !returnUrl.Contains("/profile"))
                await _returnUrlRepository.SetAsync(returnUrl);
            else
                returnUrl = await _returnUrlRepository.GetAsync();
            if (string.IsNullOrWhiteSpace(returnUrl))
                returnUrl = Url.Content("~/profile");
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = await _returnUrlRepository.GetAsync();
            if (string.IsNullOrWhiteSpace(returnUrl))
                returnUrl = Url.Content("~/profile");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email?.Trim()) ?? await _userManager.FindByNameAsync(Input.Email?.Trim());
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Не удачная попытка входа.");
                    return Page();
                }
                var result = await _signInManager.PasswordSignInAsync(user.UserName, Input.Password?.Trim(), Input.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    ModelState.AddModelError(string.Empty, "Пользователь заблокирован на 5 минут.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Не удачная попытка входа.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
