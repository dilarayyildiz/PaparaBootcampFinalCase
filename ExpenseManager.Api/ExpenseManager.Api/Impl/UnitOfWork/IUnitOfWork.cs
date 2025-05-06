using ExpenseManager.Api.Entities;
using ExpenseManager.Api.Impl.GenericRepository;

namespace ExpenseManager.Api.Impl.UnitOfWork;

public interface IUnitOfWork : IDisposable
{ 
    Task Complete(CancellationToken cancellationToken);
    IGenericRepository<User> UserRepository { get; }
    IGenericRepository<Expense> ExpenseRepository { get; }
    IGenericRepository<ExpenseCategory> ExpenseCategoryRepository { get; }
    IGenericRepository<AccountHistory> AccountHistoryRepository { get; }

}