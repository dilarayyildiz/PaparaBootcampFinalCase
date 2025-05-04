using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace ExpenseManager.Base.Token;

public class JwtExtension
{
    public static AppSession GetSession(HttpContext context)
    {
        AppSession session = new AppSession();
        var identity = context?.User?.Identity as ClaimsIdentity;
        if (identity == null)
            return session;

        var claims = identity.Claims;
        session.UserId = GetClaimValue(claims, "UserId");
        session.UserName = GetClaimValue(claims, ClaimTypes.Name);
        session.Email = GetClaimValue(claims, ClaimTypes.Email);
        session.UserRole = GetClaimValue(claims, ClaimTypes.Role);
        session.HttpContext = context;
        return session;
    }

    private static string GetClaimValue(IEnumerable<Claim> claims, string type)
    {
        var claim = claims.FirstOrDefault(c => c.Type == type);
        return claim?.Value;
    }
}