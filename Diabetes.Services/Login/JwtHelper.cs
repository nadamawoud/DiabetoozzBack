using System.Linq;
using System.Security.Claims;

namespace Diabetes.Services.Login
{
    public static class JwtHelper
    {
        public static int GetUserIdFromToken(ClaimsPrincipal user)
        {
            var claim = user.Claims.FirstOrDefault(c => c.Type == "UserId");
            return claim != null ? int.Parse(claim.Value) : 0;
        }
    }
}