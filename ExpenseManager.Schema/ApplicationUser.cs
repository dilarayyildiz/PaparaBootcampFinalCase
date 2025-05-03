namespace ExpenseManager.Schema;

public class ApplicationUserRequest
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; } // şifre plain gelir, backend hashler
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class ApplicationUserResponse
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime OpenDate { get; set; }
    public DateTime? LastLoginDate { get; set; }
}