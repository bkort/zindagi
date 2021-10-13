using Microsoft.AspNetCore.Http;

namespace Zindagi.Infra.Auth
{
    public struct AuthConstants
    {
        public const string AuthenticationScheme = "ZindagiAuthProvider";
        public const string ClaimsIssuer = "ZindagiAuthIssuer";

        public static readonly string CallbackPath = new PathString("/callback");
    }
}
