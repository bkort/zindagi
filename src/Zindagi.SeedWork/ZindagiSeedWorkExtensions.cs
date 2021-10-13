using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Zindagi.SeedWork;

namespace Zindagi
{
    public static class ZindagiSeedWorkExtensions
    {
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNotNullOrWhiteSpace(this string? value) => !string.IsNullOrWhiteSpace(value);

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetCleanString(this string? value) => string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetDescription<T>(this T enumerationValue) where T : struct
        {
            var type = enumerationValue.GetType();
            if (!type.IsEnum)
                throw new ArgumentException($"{nameof(enumerationValue)} must be of Enum type", nameof(enumerationValue));
            var memberInfo = type.GetMember(enumerationValue.ToString() ?? string.Empty);
            if (memberInfo.Length <= 0)
                return enumerationValue.ToString() ?? string.Empty;

            var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attrs.Length > 0)
                return ((DescriptionAttribute)attrs[0]).Description;
            return enumerationValue.ToString() ?? string.Empty;
        }

        public static Result<VendorId> GetIdentifier(this ClaimsPrincipal claimsPrincipal)
        {
            var nameIdentifier = claimsPrincipal.Claims.FirstOrDefault(q => q.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase))?.Value ?? string.Empty;
            return VendorId.Create(nameIdentifier);
        }

        public static Result<string> GetNameIdentifier(this ClaimsPrincipal claimsPrincipal) => claimsPrincipal.Claims.FirstOrDefault(q => q.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase))?.Value ?? string.Empty;

        public static Result<Guid> GetGuid(this ClaimsPrincipal claimsPrincipal)
        {
            var id = claimsPrincipal.Claims.FirstOrDefault(q => q.Type.Equals(ClaimTypes.Upn, StringComparison.Ordinal))?.Value ?? string.Empty;
            return Guid.Parse(id);
        }
    }
}
