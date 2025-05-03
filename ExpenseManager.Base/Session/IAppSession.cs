using Microsoft.AspNetCore.Http;

namespace ExpenseManager.Base;

public interface IAppSession
{
    string UserName { get; set; }
    string Token { get; set; }
    string UserId { get; set; }
    string UserRole { get; set; }
    string FirstName { get; set; }
    string LastName { get; set; }
    HttpContext HttpContext { get; set; }
}