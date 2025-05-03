namespace ExpenseManager.Schema;

public class AuthorizationRequest 
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class AuthorizationResponse 
{
    public string Token { get; set; }
    public string UserName { get; set; }
    public DateTime Expiration { get; set; }
}

public class ChangePasswordRequest 
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}