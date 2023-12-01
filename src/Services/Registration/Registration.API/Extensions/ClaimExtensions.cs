using System.Security.Claims;

namespace Registration.API.Extensions
{
    public static class ClaimExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId != null ? Guid.Parse(userId) : Guid.Empty;
        }
    }
}
