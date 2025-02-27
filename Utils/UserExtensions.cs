using System.Security.Claims;

namespace Utils
{
    public static class UserExtensions
    {
        public static Guid Id(this ClaimsPrincipal user, string claimName = "Id")
        {
            return user.HasClaim(c => c.Type == claimName) ? Guid.Parse(GetClaimByName(user, claimName)) : Guid.Empty;
        }
        public static string GetClaimByName(this ClaimsPrincipal user, string claimName)
        {
            return user.HasClaim(c => StandardizeClaimName(c.Type) == StandardizeClaimName(claimName) && !String.IsNullOrEmpty(c.Value))
                ? user.Claims.FirstOrDefault(c => StandardizeClaimName(c.Type) == StandardizeClaimName(claimName) && !String.IsNullOrEmpty(c.Value))!.Value
                : String.Empty;
        }
        public static string StandardizeClaimName(string claimName)
        {
            return claimName.ToLower()
                .Replace("_", String.Empty)
                .Replace("-", String.Empty)
                .Replace(".", String.Empty)
                .Replace(",", String.Empty)
                .Replace("[", String.Empty)
                .Replace("]", String.Empty)
                .Replace("(", String.Empty)
                .Replace(")", String.Empty);
        }
    }
}
