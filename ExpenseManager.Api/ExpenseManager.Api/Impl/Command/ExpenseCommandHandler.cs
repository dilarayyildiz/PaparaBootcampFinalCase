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
    IRequestHandler<CancelExpenseCommand, ApiResponse>,
    IRequestHandler<ApproveExpenseCommand, ApiResponse>,
    IRequestHandler<RejectExpenseCommand, ApiResponse>
{
    private readonly ExpenseManagerDbContext dbContext;
    private readonly IMapper mapper;
    private readonly IExpensePaymentTransactionService _expensePaymentTransactionService;

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
        expense.ReceiptUrl = request.ReceptUrl;
        
        //  Dosya varsa kaydet
        if (request.Expense.ReceiptFile != null && request.Expense.ReceiptFile.Length > 0)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "receipts");
            var uniqueFileName = $"{Guid.NewGuid()}_{request.Expense.ReceiptFile.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await request.Expense.ReceiptFile.CopyToAsync(fileStream, cancellationToken);
            }

            expense.ReceiptUrl = $"/receipts/{uniqueFileName}";
        }
        

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
    
    public async Task<ApiResponse> Handle (ApproveExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await dbContext.Set<Expense>().FirstOrDefaultAsync(x => x.Id == request.ExpenseId, cancellationToken);
        if (expense == null)
            return new ApiResponse("Expense not found");

        var user = await dbContext.Set<User>()
            .FirstOrDefaultAsync(x => x.Id == expense.UserId, cancellationToken);

        if (user != null)
        {
            await _expensePaymentTransactionService.ExpensePaymentTransactionAsync(user.Id, expense.Amount, user.IBAN, cancellationToken);
        }
        else
        {
            return new ApiResponse("payment failed");
        }
         
        expense.ExpenseStatus = ExpenseStatus.Approved;
        //DEğiştirilecek cons olarak eklenecek 
        expense.PaymentMethod = "EFT";

        dbContext.Set<Expense>().Update(expense);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse();
    }
    
    public async Task<ApiResponse> Handle (RejectExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await dbContext.Set<Expense>().FirstOrDefaultAsync(x => x.Id == request.ExpenseId, cancellationToken);
        if (expense == null)
            return new ApiResponse("Expense not found");

        expense.RejectionReason = request.RejectionReason;
        expense.ExpenseStatus = ExpenseStatus.Rejected;

        dbContext.Set<Expense>().Update(expense);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse();
    }
    
    public async Task<ApiResponse> Handle(CancelExpenseCommand request, CancellationToken cancellationToken)
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
