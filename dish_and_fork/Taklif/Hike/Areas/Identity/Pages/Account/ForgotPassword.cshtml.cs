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
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<HikeUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IReturnUrlRepository _returnUrlRepository;

        public ForgotPasswordModel(UserManager<HikeUser> userManager, IEmailSender emailSender, IReturnUrlRepository returnUrlRepository)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var returnUrl = await _returnUrlRepository.GetAsync();
                if (string.IsNullOrWhiteSpace(returnUrl))
                    returnUrl = Url.Content("~/profile");
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code, returnUrl = returnUrl },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Сброс пароля",
                    $"Вы можете сбросить свой пароль <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>нажав сюда</a>.");

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
