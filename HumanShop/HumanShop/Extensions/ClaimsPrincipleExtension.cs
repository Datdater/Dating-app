using System.Security.Claims;

namespace HumanShop.Extensions
{
    public static class ClaimsPrincipleExtension
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            var username = user.FindFirstValue(ClaimTypes.NameIdentifier)
            ??
                throw new Exception("Can't get username from token");
            return username;
        }
    }
}
