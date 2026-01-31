

namespace Hike.Entities
{
    public class BuyerInfoDto
    {
        public string Id { get; set; }
        public string? UserName { get; set; }
        public string? NormalizedUserName { get; set; }
        public string? Email { get; set; }
        public string? NormalizedEmail { get; set; }
        public bool? EmailConfirmed { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? PhoneNumberConfirmed { get; set; }
        public bool? TwoFactorEnabled { get; set; }

        //public BuyerInfo ToBuyerInfo() =>
        //    new BuyerInfo(
        //        Id,
        //        UserName,
        //        Email,
        //        EmailConfirmed ?? false,
        //        PhoneNumber,
        //        PhoneNumberConfirmed ?? false
        //        );

        //public BuyerInfoDto()
        //{

        //}

        //public BuyerInfoDto(BuyerInfo dto)
        //{
        //    Id = dto.Id;
        //    UserName = dto.UserName;
        //    NormalizedUserName = UserName?.GetNormalizedName();
        //    Email = dto.Email;
        //    NormalizedEmail = Email?.GetNormalizedName();
        //    EmailConfirmed = dto.EmailConfirmed;
        //    PhoneNumber = dto.PhoneNumber;
        //    PhoneNumberConfirmed = dto.PhoneNumberConfirmed;
       // }

        public static BuyerInfoDto From(HikeUser dto)
        {
            if (dto == null)
                return null;
            var res = new BuyerInfoDto
            {
                Id = dto.Id
            };
            res.UserName = dto.UserName;
            res.NormalizedUserName = dto.NormalizedUserName;
            res.Email = dto.Email;
            res.NormalizedEmail = dto.NormalizedEmail;
            res.EmailConfirmed = dto.EmailConfirmed;
            res.PhoneNumber = dto.PhoneNumber;
            res.PhoneNumberConfirmed = dto.PhoneNumberConfirmed;
            res.TwoFactorEnabled = dto.TwoFactorEnabled;
            return res;
        }
    }
}
