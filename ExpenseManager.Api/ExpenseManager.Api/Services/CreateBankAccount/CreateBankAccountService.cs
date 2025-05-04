using ExpenseManager.Api.Entities;
namespace ExpenseManager.Api.Services.CreateBankAccount;

public class CreateBankAccountService
{
    
    private const string TurkishLiraCurruencyCode = "TL";
    private const string ABDollarCurruencyCode = "USD";
    private const string EuroCurruencyCode = "EUR";
    
    private readonly ExpenseManagerDbContext _dbContext; 

    public CreateBankAccountService(ExpenseManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateBankAccountAsync(string firstName, string lastName, string iban, CancellationToken cancellationToken)
    { 
        var random = new Random();
        var account = new Account()
        { 
            CustomerId = (random.Next(100000, 999999)) * 1000000L + random.Next(100000, 999999),
            FirstName = firstName,
            LastName = lastName,
            IBAN = iban,
            Balance = 0,
            Currency = TurkishLiraCurruencyCode,
            OpenDate = DateTime.UtcNow,
            CloseDate = null
        };

        await _dbContext.Set<Account>().AddAsync(account, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}