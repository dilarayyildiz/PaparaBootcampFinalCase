using ExpenseManager.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.Api.Services.AccountHistory;

public class AccountHistoryService : IAccountHistoryService
{
    private readonly ExpenseManagerDbContext _dbContext;

    public AccountHistoryService(ExpenseManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateHistoryAsync(int userId, decimal transactionAmount, string iban, CancellationToken cancellationToken)
    {
        // IBAN bazında mevcut toplam bakiyeyi hesapla
        decimal totalBalance = await _dbContext.Set<Entities.AccountHistory>()
            .Where(x => x.IBAN == iban && x.IsActive)
            .SumAsync(x => x.TransactionAmount, cancellationToken);

        decimal updatedBalance = totalBalance + transactionAmount;

        var history = new Entities.AccountHistory
        {
            Balance = updatedBalance,
            TransactionAmount = transactionAmount,
            TransactionDate = DateTime.UtcNow,
            IBAN = iban,
            ReferenceNumber = Guid.NewGuid(),
            IsActive = true
        };

        await _dbContext.Set<Entities.AccountHistory>().AddAsync(history, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}