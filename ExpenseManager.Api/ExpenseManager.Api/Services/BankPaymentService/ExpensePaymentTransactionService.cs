using ExpenseManager.Api.Entities;
using ExpenseManager.Api.Impl.UnitOfWork;
using ExpenseManager.Api.Services.AccountHistory;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.Api.Services.BankPaymentService;

public class ExpensePaymentTransactionService : IExpensePaymentTransactionService
{
    private readonly IUnitOfWork unitOfWork;
    private const string CompanyIBAN = "TR340000000000000000000000";

    public ExpensePaymentTransactionService(IUnitOfWork unitOfWork)
    { 
        this.unitOfWork = unitOfWork;
    }

    public async Task ExpensePaymentTransactionAsync(int userId, decimal transactionAmount, string iban, CancellationToken cancellationToken)
    {
        // IBAN bazında mevcut toplam bakiyeyi hesapla
        decimal totalBalance = await unitOfWork.AccountHistoryRepository 
            .SumAsync(x => x.ToIBAN == iban && x.IsActive,x => x.TransactionAmount);

        decimal updatedBalance = totalBalance + transactionAmount;

        var history = new Entities.AccountHistory
        {
            BalanceAfterTransaction = updatedBalance,
            TransactionAmount = transactionAmount,
            TransactionDate = DateTime.UtcNow,
            ToIBAN  = iban,
            FromIBAN = CompanyIBAN,
            ReferenceNumber = Guid.NewGuid(),
            IsActive = true
        };

        await unitOfWork.AccountHistoryRepository.AddAsync(history, cancellationToken);
        await unitOfWork.Complete(cancellationToken);
    }
}