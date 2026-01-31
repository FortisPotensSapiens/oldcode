using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Hike
{
    public class HikeAuthOption : IConfigureOptions<ApiAuthorizationOptions>
    {
        private readonly ISigningCredentialStore _store;
        private readonly IConfiguration _configuration;

        public HikeAuthOption(ISigningCredentialStore store, IConfiguration configuration)
        {
            _store = store;
            _configuration = configuration;
        }

        public void Configure(ApiAuthorizationOptions options)
        {
            options.ApiResources = new ApiResourceCollection(new List<ApiResource>()
            {
                new ApiResource("HikeAPI")
                {
                    Enabled = true,
                    Scopes = new List<string>(){ "HikeAPI" },
                    ShowInDiscoveryDocument = true,
                    Properties =
                    {
                        ["Profile"] = "IdentityServerJwt",
                        ["Source"] = "Configuration",
                        ["Clients"] = "*",
                    }
                }
            });

            options.IdentityResources = new IdentityResourceCollection(new List<IdentityResource>
            {
                IdentityResourceBuilder.OpenId().AllowAllClients().Build(),
                IdentityResourceBuilder.Profile().AllowAllClients().Build(),
            });
            var c = ClientBuilder.IdentityServerSPA("Hike")
                .WithRedirectUri("/authentication/login-callback")
                .WithLogoutRedirectUri("/authentication/logout-callback")
                .Build();
            c.AllowOfflineAccess = true;
            c.AllowedScopes.Add("offline_access");
            c.RefreshTokenUsage = TokenUsage.OneTimeOnly;
            c.AbsoluteRefreshTokenLifetime = 25_920_000;
            c.UpdateAccessTokenClaimsOnRefresh = true;
            var c2 = ClientBuilder.NativeApp("HikeMobile")
                .WithRedirectUri("dnfbuyer://authentication/login-callback")
                .WithLogoutRedirectUri("dnfbuyer://authentication/logout-callback")
                .Build();
            c2.AllowOfflineAccess = true;
            c2.AllowedScopes.Add("offline_access");
            c2.RefreshTokenUsage = TokenUsage.OneTimeOnly;
            c2.AbsoluteRefreshTokenLifetime = 25_920_000;
            c2.UpdateAccessTokenClaimsOnRefresh = true;
            var c3 = ClientBuilder.SPA("HikeMobile2")
                .WithRedirectUri("dnfbuyer://authentication/login-callback")
                .WithLogoutRedirectUri("dnfbuyer://authentication/logout-callback")
                .Build();
            c3.AllowOfflineAccess = true;
            c3.AllowedScopes.Add("offline_access");
            c3.RefreshTokenUsage = TokenUsage.OneTimeOnly;
            c3.AbsoluteRefreshTokenLifetime = 25_920_000;
            c3.UpdateAccessTokenClaimsOnRefresh = true;
            var c4 = ClientBuilder.SPA("HikeHybrid")
                .WithRedirectUri("dnfbuyer://authentication/login-callback")
                .WithLogoutRedirectUri("dnfbuyer://authentication/logout-callback")
                .Build();
            c4.AllowOfflineAccess = true;
            c4.AllowedScopes.Add("offline_access");
            c4.RefreshTokenUsage = TokenUsage.OneTimeOnly;
            c4.AbsoluteRefreshTokenLifetime = 25_920_000;
            c4.UpdateAccessTokenClaimsOnRefresh = true;
            c4.AllowedGrantTypes = GrantTypes.Hybrid;
            if (_configuration["IsTesting"]?.Trim().ToLower() == "true")
            {
                var api = ClientBuilder.SPA("DafTest")
                    .WithRedirectUri("http://localhost:3000/authentication/login-callback")
                    .WithLogoutRedirectUri("http://localhost:3000/authentication/logout-callback")
                    .Build();
                api.AllowOfflineAccess = true;
                api.AllowedScopes.Add("offline_access");
                api.RefreshTokenUsage = TokenUsage.OneTimeOnly;
                api.AbsoluteRefreshTokenLifetime = 25_920_000;
                api.UpdateAccessTokenClaimsOnRefresh = true;
                var api2 = ClientBuilder.SPA("HikeTest")
                  .WithRedirectUri("https://localhost:44396/authentication/login-callback")
                  .WithLogoutRedirectUri("https://localhost:44396/authentication/logout-callback")
                  .Build();
                api2.AllowOfflineAccess = true;
                api2.AllowedScopes.Add("offline_access");
                api2.RefreshTokenUsage = TokenUsage.OneTimeOnly;
                api2.AbsoluteRefreshTokenLifetime = 25_920_000;
                api2.UpdateAccessTokenClaimsOnRefresh = true;
                options.Clients = new ClientCollection(new List<Client>() { c, c2, c3, c4, api, api2 });
            }
            else
            {
                options.Clients = new ClientCollection(new List<Client>() { c, c2, c3, c4 });
            }

            options.SigningCredential =
                _store.GetSigningCredentialsAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
