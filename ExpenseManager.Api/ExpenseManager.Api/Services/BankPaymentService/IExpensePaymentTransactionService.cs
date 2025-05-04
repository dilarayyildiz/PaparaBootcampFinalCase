namespace ExpenseManager.Api.Services.AccountHistory;

public interface IExpensePaymentTransactionService
{
    Task ExpensePaymentTransactionAsync(int userId, decimal transactionAmount, string iban, CancellationToken cancellationToken);
}