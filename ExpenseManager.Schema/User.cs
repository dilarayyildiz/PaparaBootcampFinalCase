namespace ExpenseManager.Schema;


public class UserRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public string Password { get; set; } // API tarafında plain gelir, backend hashler
    public string Role { get; set; }     // "Admin" veya "Personnel"
    public string IBAN { get; set; }
}

//PasswordHash yerine API dışından plain Password alırız, backend (handler/service) içinde hashleriz.
//Role genelde string gelir (örn. "Admin"), backend Enum.Parse<UserRole>(role) yapar.

public class UserResponse
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public string Role { get; set; }     // enum stringe dönüştürülür
    public string IBAN { get; set; }
    public bool IsActive { get; set; }
}

//Sensitive alanlar (örn. PasswordHash) kesinlikle dışarı çıkmaz.
//Role enum, response tarafında okunabilir olsun diye string döner (örn. "Admin").
