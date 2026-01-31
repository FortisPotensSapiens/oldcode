using Microsoft.AspNetCore.Authentication;

namespace Hike.Models
{
    public class AuthenticationSchemeReadModel
    {
        //
        // Сводка:
        //     The name of the authentication scheme.
        public string Name { get; set; }
        //
        // Сводка:
        //     The display name for the scheme. Null is valid and used for non user facing schemes.
        public string? DisplayName { get; set; }
        public static AuthenticationSchemeReadModel From(AuthenticationScheme dto)
        {
            if (dto == null)
                return null;
            return new AuthenticationSchemeReadModel
            {
                DisplayName = dto.DisplayName,
                Name = dto.Name
            };
        }
    }
}
