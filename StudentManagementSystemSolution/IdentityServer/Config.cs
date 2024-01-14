using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource("roles", "Roles", new List<string> { "role" }),
            new IdentityResource("permissions", "Permissions", new List<string> { "permission" })
            //new IdentityResource("roles", new[] { "role" })
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
            new ApiScope("attendance.full_access"),
            };

        public static IEnumerable<ApiResource> GetApiResources()
        {
         return new List<ApiResource>
            {
                new ApiResource("attendance", "Attendance API")
                {
                    Scopes = { "attendance.full_access" },
                    UserClaims = {"role", "student", "class", "teacher"}
                },
            };
        }

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
            //// m2m client credentials flow client
            //new Client
            //{
            //    ClientId = "m2m.client",
            //    ClientName = "Client Credentials Client",

            //    AllowedGrantTypes = GrantTypes.ClientCredentials,
            //    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

            //    AllowedScopes = { "scope1" }
            //},

            //// interactive client using code flow + pkce
            //new Client
            //{
            //    ClientId = "interactive",
            //    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

            //    AllowedGrantTypes = GrantTypes.Code,

            //    RedirectUris = { "https://localhost:44300/signin-oidc" },
            //    FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
            //    PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

            //    AllowOfflineAccess = true,
            //    AllowedScopes = { "openid", "profile", "scope2" }
            //},

            new Client
            {
                ClientId = "SMSClient",
                ClientSecrets = { new Secret("SMSSecret".Sha256())},

                AllowedGrantTypes = GrantTypes.Code,
                AlwaysIncludeUserClaimsInIdToken = true,

                RequireConsent = true,

                RedirectUris = { "https://localhost:5002/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:5002/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

                AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, "permissions", "roles", "attendance.full_access" }
            }

            };
    }
}