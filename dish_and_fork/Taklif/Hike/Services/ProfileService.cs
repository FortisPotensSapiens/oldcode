using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Hike.Entities;
using IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace Hike
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<HikeUser> _userManager;

        public ProfileService(UserManager<HikeUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));
            var subjectId = subject.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            var user = await _userManager.FindByIdAsync(subjectId);
            if (user == null)
                throw new ApplicationException("Invalid subject identifier");
            var roles = await _userManager.GetRolesAsync(user);
            var claims = GetClaimsFromUser(user);
            context.IssuedClaims = claims.ToList();
            foreach (var role in roles)
            {
                context.IssuedClaims.Add(new Claim(JwtClaimTypes.Role, role));
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));
            var subjectId = subject.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            var user = await _userManager.FindByIdAsync(subjectId);
            context.IsActive = false;
            if (user != null)
            {
                if (_userManager.SupportsUserSecurityStamp)
                {
                    var security_stamp = subject.Claims.Where(c => c.Type == "security_stamp").Select(c => c.Value).SingleOrDefault();
                    if (security_stamp != null)
                    {
                        var db_security_stamp = await _userManager.GetSecurityStampAsync(user);
                        if (db_security_stamp != security_stamp)
                            return;
                    }
                }
                context.IsActive =
                    !user.LockoutEnabled ||
                    !user.LockoutEnd.HasValue ||
                    user.LockoutEnd <= DateTime.Now;
            }
        }

        private IEnumerable<Claim> GetClaimsFromUser(HikeUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, user.Id),
                new Claim(JwtClaimTypes.PreferredUserName, user.UserName),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),
                new Claim(JwtClaimTypes.Name, user.Id),
                new Claim(nameof(HikeUser.AcceptedConsentToMailings), user.AcceptedConsentToMailings.ToString()),
                new Claim(nameof(HikeUser.AcceptedPivacyPolicy), user.AcceptedPivacyPolicy.ToString()),
                new Claim(nameof(HikeUser.AcceptedConsentToPersonalDataProcessing), user.AcceptedConsentToPersonalDataProcessing.ToString()),
                  new Claim(nameof(HikeUser.AcceptedOfferFoUser), user.AcceptedOfferFoUser.ToString()),
                    new Claim(nameof(HikeUser.AcceptedTermsOfUse), user.AcceptedTermsOfUse.ToString()),
            };
            if (_userManager.SupportsUserEmail)
            {
                claims.AddRange(new[]
                {
                    new Claim(JwtClaimTypes.Email, user.Email),
                    new Claim(JwtClaimTypes.EmailVerified, user.EmailConfirmed? "true" : "false", ClaimValueTypes.Boolean)
                });
            }
            if (_userManager.SupportsUserPhoneNumber && !string.IsNullOrWhiteSpace(user.PhoneNumber))
            {
                claims.AddRange(new[]
                {
                    new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber),
                    new Claim(JwtClaimTypes.PhoneNumberVerified, user.PhoneNumberConfirmed ? "true" : "false", ClaimValueTypes.Boolean)
                });
            }
            return claims;
        }
    }
}
