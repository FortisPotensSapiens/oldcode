using ApeBotApi.Attributes;
using ApeBotApi.DbDtos;
namespace AleBotApi.Models
{
    public sealed class UserReadModel
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? Telegram { get; set; }
        [PhoneNumberValidation]
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public byte[]? Photo { get; set; }
        public string[]? Roles { get; set; }
        public Guid? RefererId { get; set; }
        public string? RefererFullName { get; set; }
        public string? RefererEmail { get; set; }


        public static UserReadModel? From(AbUserDbDto? dto)
        {
            if (dto is null)
                return null;
            return new UserReadModel
            {
                Id = dto.Id,
                FullName = dto.FullName,
                Email = dto.Email,
                EmailConfirmed = dto.EmailConfirmed,
                Telegram = dto.Telegram,
                PhoneNumber = dto.PhoneNumber,
                Photo = dto.Photo,
                Roles = dto.Roles?.Select(x => x.Role?.NormalizedName).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray(),
                PhoneNumberConfirmed = dto.PhoneNumberConfirmed,
                RefererId = dto.RefererId,
                RefererFullName = dto.Referer?.FullName,
                RefererEmail = dto.Referer?.Email
            };
        }
    }
}
