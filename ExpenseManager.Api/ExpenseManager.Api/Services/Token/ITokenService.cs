using ExpenseManager.Api.Entities;

namespace ExpenseManager.Api.Services.Token;

public interface ITokenService
{
    public string GenerateToken(User user);
   // public string GenerateToken(ApplicationUser user);
}