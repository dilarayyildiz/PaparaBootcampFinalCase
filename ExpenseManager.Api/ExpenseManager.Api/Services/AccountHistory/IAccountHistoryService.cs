namespace ExpenseManager.Api.Services.AccountHistory;

public interface IAccountHistoryService
{
    Task CreateHistoryAsync(int userId, decimal transactionAmount, string iban, CancellationToken cancellationToken);
}