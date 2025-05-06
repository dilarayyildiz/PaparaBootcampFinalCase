using ExpenseManager.Api.Entities;
using ExpenseManager.Api.Impl.GenericRepository;
using Serilog;

namespace ExpenseManager.Api.Impl.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ExpenseManagerDbContext dbContext;

    private IGenericRepository<User> _userRepository;
    private IGenericRepository<Expense> _expenseRepository;
    private IGenericRepository<ExpenseCategory> _expenseCategoryRepository;
    private IGenericRepository<Account> _accountRepository;
    private IGenericRepository<AccountHistory> _accountHistoryRepository;

    public UnitOfWork(ExpenseManagerDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public IGenericRepository<User> UserRepository => _userRepository ??= new GenericRepository<User>(dbContext);
    public IGenericRepository<Expense> ExpenseRepository => _expenseRepository ??= new GenericRepository<Expense>(dbContext);
    public IGenericRepository<ExpenseCategory> ExpenseCategoryRepository => _expenseCategoryRepository ??= new GenericRepository<ExpenseCategory>(dbContext);
    public IGenericRepository<Account> AccountRepository => _accountRepository ??= new GenericRepository<Account>(dbContext);
    public IGenericRepository<AccountHistory> AccountHistoryRepository => _accountHistoryRepository ??= new GenericRepository<AccountHistory>(dbContext);

    public async Task Complete(CancellationToken cancellationToken)
    {
        using (var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken))
        {
            try
            {
                await dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while saving changes to the database.");
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }

    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                dbContext.Dispose();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}