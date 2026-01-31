using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Hike.Ef;
using Hike.Entities;
using Hike.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hike.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<HikeUser> _signInManager;
        private readonly UserManager<HikeUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly HikeDbContext _db;
        private readonly IReturnUrlRepository _returnUrlRepository;

        public RegisterModel(
            UserManager<HikeUser> userManager,
            SignInManager<HikeUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            HikeDbContext db,
            IReturnUrlRepository returnUrlRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _db = db;
            _returnUrlRepository = returnUrlRepository;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Укажите {0}")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Укажите {0}")]
            [StringLength(100, ErrorMessage = "{0} должен быть длинной минимум {2} и максимум {1} символов.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Подтверждение пароля")]
            [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
            public string ConfirmPassword { get; set; }

            //[Display(Name = "Я принимаю согласие о персональных данных?")]
            //[Range(typeof(bool), "True", "True", ErrorMessage = "Примите политику конфенденциальности")]
            //public bool AcceptTermsOfService { get; set; }
            //[Display(Name = "Вы являетесь продавцом? Можно будет позже в профиле поставить эту галочку в профиле чтобы иметь возможность выставлять свои услуги и товары на продажу на нашем сайте.")]
            //public bool IsSeller { get; set; }

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

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) && !returnUrl.Contains("/profile"))
                await _returnUrlRepository.SetAsync(returnUrl);
            else
                returnUrl = await _returnUrlRepository.GetAsync();
            if (string.IsNullOrWhiteSpace(returnUrl))
                returnUrl = Url.Content("~/profile");
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = await _returnUrlRepository.GetAsync();
            if (string.IsNullOrWhiteSpace(returnUrl))
                returnUrl = Url.Content("~/profile");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
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
                using (var transaction = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable))
                {
                    var user = new HikeUser
                    {
                        UserName = Input.Email?.Trim(),
                        Email = Input.Email?.Trim(),
                        AcceptedConsentToMailings = Input.AcceptedConsentToMailings,
                        AcceptedConsentToPersonalDataProcessing = Input.AcceptedConsentToPersonalDataProcessing,
                        AcceptedPivacyPolicy = Input.AcceptedPivacyPolicy,
                        AcceptedOfferFoUser = Input.AcceptedOfferFoUser,
                        AcceptedTermsOfUse = Input.AcceptedTermsOfUser,
                    };
                    var result = await _userManager.CreateAsync(user, Input.Password?.Trim());
                    //if (result.Succeeded)
                    //    result = await _userManager.AddToRoleAsync(user, "seller");
                    if (result.Succeeded)
                    {
                        await _db.SaveChangesAsync();
                        _logger.LogInformation("Пользователь создал новый аккаунт");
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);
                        await _emailSender.SendEmailAsync(Input.Email, "Потверждение почты",
                            $"Пожалуйста потвердите свой аккаунт: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Нажмите сюда</a>.");
                        await transaction.CommitAsync();
                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("RegisterConfirmation",
                                new { email = Input.Email, returnUrl = returnUrl });
                        }
                        else
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return Page();
        }
    }
}
