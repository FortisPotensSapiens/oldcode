using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace Hike
{
    public class JwtBearerOptionsConfig : IConfigureNamedOptions<JwtBearerOptions>
    {
        public void Configure(JwtBearerOptions options)
        {
            options.TokenValidationParameters.ValidateIssuer = false;
        }

        public void Configure(string name, JwtBearerOptions options)
        {
            options.TokenValidationParameters.ValidateIssuer = false;
            options.TokenValidationParameters.ValidateIssuerSigningKey = false;
        }
    }
}