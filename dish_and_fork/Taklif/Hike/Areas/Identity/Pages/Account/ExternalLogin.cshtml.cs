using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Hike.Ef;
using Hike.Entities;
using Hike.Extensions;
using Hike.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Hike.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<HikeUser> _signInManager;
        private readonly UserManager<HikeUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IReturnUrlRepository _returnUrlRepository;
        private readonly HikeDbContext _db;
        private readonly ILogger<ExternalLoginModel> _logger;

        public ExternalLoginModel(
            SignInManager<HikeUser> signInManager,
            UserManager<HikeUser> userManager,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender,
            IReturnUrlRepository returnUrlRepository,
            HikeDbContext db)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
            _returnUrlRepository = returnUrlRepository;
            _db = db;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ProviderDisplayName { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Укажите Email")]
            [EmailAddress(ErrorMessage = "Не валидный Email")]
            public string Email { get; set; }

            /// <summary>
            /// Пользователь согласился на обработку его персональных данных
            /// </summary>
            [Display(Name = "Я принимаю согласие на обработку персональных данных?")]
            //[Range(typeof(bool), "true", "true", ErrorMessage = "Примите согласился на обработку персональных данных")]
            public bool AcceptedConsentToPersonalDataProcessing { get; set; }
            /// <summary>
            /// Пользователь принял политику конфеденциальности
            /// </summary>
            [Display(Name = "Я принимаю политику конфеденциальности?")]
            //[Range(typeof(bool), "true", "true", ErrorMessage = "Примите политику конфеденциальности")]
            public bool AcceptedPivacyPolicy { get; set; }
            /// <summary>
            /// Пользователь принял согласие на рассылку 
            /// </summary>
            [Display(Name = "Я принимаю согласие на рассылку мне новостных, маркентинговых и иных данных?")]
            public bool AcceptedConsentToMailings { get; set; }
            /// <summary>
            /// Пользователь принял условия использования
            /// </summary>
            [Display(Name = "Я принимаю условия использования?")]
            public bool AcceptedTermsOfUser { get; set; }
            /// <summary>
            /// Пользователь принял оферту для физических лиц 
            /// </summary>
            [Display(Name = "Я принимаю оферту для физических лиц на заключение договоров купли-продажи?")]
            public bool AcceptedOfferFoUser { get; set; }
        }

        public IActionResult OnGetAsync()
        {
            return RedirectToPage("./Login");
        }

        public async Task<IActionResult> OnPostAsync(string provider, string returnUrl = null)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) && !returnUrl.Contains("/profile"))
                await _returnUrlRepository.SetAsync(returnUrl);
            else
                returnUrl = await _returnUrlRepository.GetAsync();
            if (string.IsNullOrWhiteSpace(returnUrl))
                returnUrl = Url.Content("~/profile");
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) && !returnUrl.Contains("/profile"))
                await _returnUrlRepository.SetAsync(returnUrl);
            else
                returnUrl = await _returnUrlRepository.GetAsync();
            if (string.IsNullOrWhiteSpace(returnUrl))
                returnUrl = Url.Content("~/profile");
            if (remoteError != null)
            {
                ErrorMessage = $"Ошибка для внешнего провайдера: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Ошибка при загрузке внешей информации для входа.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                ProviderDisplayName = info.ProviderDisplayName;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) && !returnUrl.Contains("/profile"))
                await _returnUrlRepository.SetAsync(returnUrl);
            else
                returnUrl = await _returnUrlRepository.GetAsync();
            if (string.IsNullOrWhiteSpace(returnUrl))
                returnUrl = Url.Content("~/profile");
            if (!ModelState.IsValid || !Input.AcceptedPivacyPolicy || !Input.AcceptedConsentToPersonalDataProcessing
               || !Input.AcceptedTermsOfUser || !Input.AcceptedOfferFoUser)
            {
                if (!Input.AcceptedPivacyPolicy)
                    ModelState.AddModelError(string.Empty, "Вы не приняли политику конфеденциальности сервиса!");
                if (!Input.AcceptedConsentToPersonalDataProcessing)
                    ModelState.AddModelError(string.Empty, "Вы не приняли согласие на обработку персональных данных!");
                if (!Input.AcceptedTermsOfUser)
                    ModelState.AddModelError(string.Empty, "Вы не приняли условия использования!");
                if (!Input.AcceptedOfferFoUser)
                    ModelState.AddModelError(string.Empty, "Вы не приняли оферту для физических лиц на заключение договоров купли-продажи!");
                return Page();
            }
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            ProviderDisplayName = info.ProviderDisplayName;
            ReturnUrl = returnUrl;
            using (var ms = new MemoryStream())
            {
                await Request.Body.CopyToAsync(ms);
                var str = ms.ToArray();
                var res = Encoding.UTF8.GetString(str);
            };
            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                user = new HikeUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    EmailConfirmed = true,
                    AcceptedConsentToMailings = Input.AcceptedConsentToMailings,
                    AcceptedConsentToPersonalDataProcessing = Input.AcceptedConsentToPersonalDataProcessing,
                    AcceptedOfferFoUser = Input.AcceptedOfferFoUser,
                    AcceptedPivacyPolicy = Input.AcceptedPivacyPolicy,
                    AcceptedTermsOfUse = Input.AcceptedTermsOfUser
                };
                var resultC = await _userManager.CreateAsync(user);
                if (!resultC.Succeeded)
                {
                    foreach (var error in resultC.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();
                }
            }
            else
            {

                user.AcceptedConsentToMailings = Input.AcceptedConsentToMailings;
                user.AcceptedConsentToPersonalDataProcessing = Input.AcceptedConsentToPersonalDataProcessing;
                user.AcceptedOfferFoUser = Input.AcceptedOfferFoUser;
                user.AcceptedPivacyPolicy = Input.AcceptedPivacyPolicy;
                user.AcceptedTermsOfUse = Input.AcceptedTermsOfUser;
                await _db.SaveChangesAsync();
            }
            var result = await _userManager.AddLoginAsync(user, info);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }
            _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
            //var userId = await _userManager.GetUserIdAsync(user);
            //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            //var callbackUrl = Url.Page(
            //    "/Account/ConfirmEmail",
            //    pageHandler: null,
            //    values: new { area = "Identity", userId = userId, code = code },
            //    protocol: Request.Scheme);

            //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
            //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            //// If account confirmation is required, we need to show the link if we don't have a real email sender
            //if (_userManager.Options.SignIn.RequireConfirmedAccount)
            //{
            //    return RedirectToPage("./RegisterConfirmation", new { Email = Input.Email });
            //}

            await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);

            return LocalRedirect(returnUrl);
        }
    }
}
