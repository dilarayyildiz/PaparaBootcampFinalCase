namespace ExpenseManager.Api.Services.CreateBankAccount;

public interface ICreateBankAccountService
{
    Task CreateBankAccountAsync(string firstName, string lastName, string iban, CancellationToken cancellationToken);
}