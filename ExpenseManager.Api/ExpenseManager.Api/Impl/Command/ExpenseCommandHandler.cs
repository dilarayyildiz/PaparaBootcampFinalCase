using AutoMapper;
using ExpenseManager.Api.Entities;
using ExpenseManager.Api.Impl.Cqrs;
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

    public ExpenseCommandHandler(ExpenseManagerDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
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
