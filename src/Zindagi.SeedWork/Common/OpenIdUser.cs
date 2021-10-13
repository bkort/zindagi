using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

#nullable disable

namespace Zindagi.SeedWork
{
    [DebuggerDisplay("{GetPersistenceKey()}: {Email} (Verified: {IsEmailVerified})")]
    public class OpenIdUser
    {
        public VendorId AuthId => VendorId.Create(NameIdentifier);
        public string NameIdentifier { get; set; }
        public string NickName { get; set; }
        public string FirstName { get; set; }
        public string PictureUrl { get; set; }
        public string Email { get; set; }
        public bool IsEmailVerified { get; set; }

        public static OpenIdUser Create(ClaimsPrincipal claimsPrincipal)
        {
            var claims = claimsPrincipal.Claims.ToArray();
            var result = new OpenIdUser
            {
                NameIdentifier = claims.FirstOrDefault(q => q.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase))?.Value ?? string.Empty,
                NickName = claims.FirstOrDefault(q => q.Type.Equals("nickname", StringComparison.OrdinalIgnoreCase))?.Value ?? string.Empty,
                Email = claims.FirstOrDefault(q => q.Type.Equals(ClaimTypes.Email, StringComparison.OrdinalIgnoreCase))?.Value ?? string.Empty,
                FirstName = claims.FirstOrDefault(q => q.Type.Equals(ClaimTypes.Name, StringComparison.OrdinalIgnoreCase))?.Value ?? string.Empty,
                IsEmailVerified = claims.FirstOrDefault(q => q.Type.Equals("email_verified", StringComparison.OrdinalIgnoreCase))?.Value.ToUpperInvariant().Equals("TRUE", StringComparison.OrdinalIgnoreCase) ?? false,
                PictureUrl = claims.FirstOrDefault(q => q.Type.Equals("picture", StringComparison.OrdinalIgnoreCase))?.Value ?? string.Empty
            };

            if (string.IsNullOrWhiteSpace(result.FirstName))
                result.FirstName = result.NickName;

            return result;
        }

        public string GetPersistenceKey() => AuthId.GetPersistenceKey();
    }
}
