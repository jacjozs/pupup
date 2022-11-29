using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace PupUp.Helpers.Extensions
{
    public static class ClaimExtensions
    {
        public static string GetClaim(this IEnumerable<Claim> claims, string name)
        {
            return claims.FirstOrDefault(c => c.Type == name)?.Value;
        }
    }
}
