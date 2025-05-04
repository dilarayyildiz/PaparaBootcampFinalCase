using ExpenseManager.Api.Entities;
using ExpenseManager.Api.Services.AccountHistory;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.Api.Services.BankPaymentService;

public class ExpensePaymentTransactionService : IExpensePaymentTransactionService
{
    private readonly ExpenseManagerDbContext _dbContext;
    private const string CompanyIBAN = "TR34 0000 0000 0000 0000 0000 00";

    public ExpensePaymentTransactionService(ExpenseManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ExpensePaymentTransactionAsync(int userId, decimal transactionAmount, string iban, CancellationToken cancellationToken)
    {
        // IBAN bazında mevcut toplam bakiyeyi hesapla
        decimal totalBalance = await _dbContext.Set<Entities.AccountHistory>()
            .Where(x => x.ToIBAN == iban && x.IsActive)
            .SumAsync(x => x.TransactionAmount, cancellationToken);

        decimal updatedBalance = totalBalance + transactionAmount;

        var history = new Entities.AccountHistory
        {
            Balance = updatedBalance,
            TransactionAmount = transactionAmount,
            TransactionDate = DateTime.UtcNow,
            ToIBAN  = iban,
            FromIBAN = CompanyIBAN,
            ReferenceNumber = Guid.NewGuid(),
            IsActive = true
        };

        await _dbContext.Set<Entities.AccountHistory>().AddAsync(history, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}