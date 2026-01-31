using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Hike.Entities;
using Hike.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace Hike.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResendEmailConfirmationModel : PageModel
    {
        private readonly UserManager<HikeUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IReturnUrlRepository _returnUrlRepository;

        public ResendEmailConfirmationModel(UserManager<HikeUser> userManager, IEmailSender emailSender, IReturnUrlRepository returnUrlRepository)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _returnUrlRepository = returnUrlRepository;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Письмо отправленно. Проверте свою почту.");
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var returnUrl = await _returnUrlRepository.GetAsync();
            if (string.IsNullOrWhiteSpace(returnUrl))
                returnUrl = Url.Content("~/profile");
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                Input.Email,
                "Подтверждение почты",
                $"Чтобы подтвердить свою почту <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>нажмите сюда</a>.");

            ModelState.AddModelError(string.Empty, "Письмо отправленно. Проверте свою почту.");
            return Page();
        }
    }
}
