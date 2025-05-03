using AutoMapper;
using ExpenseManager.Api.Entities;
using ExpenseManager.Api.Impl.Cqrs;
using ExpenseManager.Api.Services.AccountHistory;
using ExpenseManager.Base.ApiResponse;
using Microsoft.EntityFrameworkCore;
using ExpenseManager.Schema;
using MediatR;

namespace ExpenseManager.Api.Impl.Command;

public class ExpenseCommandHandler :
    IRequestHandler<CreateExpenseCommand, ApiResponse<ExpenseResponse>>,
    IRequestHandler<UpdateExpenseCommand, ApiResponse>,
    IRequestHandler<DeleteExpenseCommand, ApiResponse>
{
    private readonly ExpenseManagerDbContext dbContext;
    private readonly IMapper mapper;
    private readonly IAccountHistoryService accountHistoryService;

    public ExpenseCommandHandler(ExpenseManagerDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
        //aslında ihtiyac yok ama gpt koy dedi.
        //this.accountHistoryService = accountHistoryService;
    }

    public async Task<ApiResponse<ExpenseResponse>> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = mapper.Map<Expense>(request.Expense);
        expense.UserId = request.Expense.UserId;
        expense.ExpenseStatus = ExpenseStatus.Pending;
        expense.IsActive = true;

        await dbContext.Set<Expense>().AddAsync(expense, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = mapper.Map<ExpenseResponse>(expense);
        return new ApiResponse<ExpenseResponse>(response);
    }

    public async Task<ApiResponse> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await dbContext.Set<Expense>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (expense == null)
            return new ApiResponse("Expense not found");

        //enum donusumu oldugu icin burada tabloda Onaylandı = 2 olacak gibi kontrol
        if (request.Expense.ExpenseStatus == ExpenseStatus.Approved.ToString())
        { 
            var user = await dbContext.Set<User>()
                .FirstOrDefaultAsync(x => x.Id == expense.UserId, cancellationToken);

            if (user != null)
            {
                await accountHistoryService.CreateHistoryAsync(user.Id, expense.Amount, user.IBAN, cancellationToken);
            }
            else
            {
                return new ApiResponse("payment failed");
            }
        }
            
        
        expense.Amount = request.Expense.Amount;
        expense.Description = request.Expense.Description;
        expense.PaymentMethod = request.Expense.PaymentMethod;
        expense.PaymentLocation = request.Expense.PaymentLocation;
        expense.CategoryId = request.Expense.CategoryId;
        expense.ExpenseStatus = Enum.Parse<ExpenseStatus>(request.Expense.ExpenseStatus);

        dbContext.Set<Expense>().Update(expense);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse();
    }
    public async Task<ApiResponse> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await dbContext.Set<Expense>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (expense == null)
            return new ApiResponse("Expense not found");

        expense.IsActive = false;

        dbContext.Set<Expense>().Update(expense);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse();
    }
}
