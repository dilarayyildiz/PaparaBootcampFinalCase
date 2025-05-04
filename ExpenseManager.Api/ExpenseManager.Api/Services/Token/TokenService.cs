using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ExpenseManager.Api.Entities;
using ExpenseManager.Base.Token;
using Microsoft.IdentityModel.Tokens;

namespace ExpenseManager.Api.Services.Token;

public class TokenService : ITokenService
{
    private readonly JwtConfig jwtConfig;

    public TokenService(JwtConfig jwtConfig)
    {
        this.jwtConfig = jwtConfig;
    }
    //public string GenerateTokenAsync(User user)
    //{
    //    string token = GenerateToken(user);
    //    return token;
    //
    //}
    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim("UserId", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtConfig.Issuer,
            audience: jwtConfig.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(jwtConfig.AccessTokenExpiration),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private Claim[] GetClaims(User user)
    {
        var claims = new List<Claim>
        {
            new Claim("Name", user.FirstName),
            new Claim("Surname", user.LastName),
            new Claim("UserId", user.Id.ToString()),
            new Claim("Email", user.Email),
            new Claim(ClaimTypes.Role,user.Role.ToString())
        };

        return claims.ToArray();
    }
}