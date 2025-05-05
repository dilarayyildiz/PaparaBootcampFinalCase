using System.Security.Claims;
using AutoMapper;
using ExpenseManager.Api.Entities;
using ExpenseManager.Api.Impl.Cqrs;
using ExpenseManager.Api.Services.AccountHistory;
using ExpenseManager.Api.Services.BankPaymentService;
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
    private readonly ExpenseManagerDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IExpensePaymentTransactionService _expensePaymentTransactionService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ExpenseCommandHandler(ExpenseManagerDbContext dbContext, IMapper mapper)
    {
        this._dbContext = dbContext;
        this._mapper = mapper;
        _httpContextAccessor = new HttpContextAccessor();
        _expensePaymentTransactionService = new ExpensePaymentTransactionService(dbContext);
        //aslında ihtiyac yok ama gpt koy dedi.
        //this.accountHistoryService = accountHistoryService;
    }

    public async Task<ApiResponse<ExpenseResponse>> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.Claims
            .FirstOrDefault(c => c.Type == "UserId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            return new ApiResponse<ExpenseResponse>("Unauthorized or missing UserId claim");
        }

        var userId = int.Parse(userIdClaim);
        
        var expense = _mapper.Map<Expense>(request.Expense);
        expense.UserId = userId;
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
        

        await _dbContext.Set<Expense>().AddAsync(expense, cancellationToken);    
        await _dbContext.SaveChangesAsync(cancellationToken);

        var response = _mapper.Map<ExpenseResponse>(expense);
        return new ApiResponse<ExpenseResponse>(response);
    }

    public async Task<ApiResponse> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await _dbContext.Set<Expense>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (expense == null)
            return new ApiResponse("Expense not found");
 
        expense.Amount = request.Expense.Amount;
        expense.Description = request.Expense.Description;
        expense.PaymentMethod = request.Expense.PaymentMethod;
        expense.PaymentLocation = request.Expense.PaymentLocation;
        expense.CategoryId = request.Expense.CategoryId;
        expense.ExpenseStatus = Enum.Parse<ExpenseStatus>(request.Expense.ExpenseStatus);
        

        _dbContext.Set<Expense>().Update(expense);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse();
    }
    
    public async Task<ApiResponse> Handle (ApproveExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await _dbContext.Set<Expense>().FirstOrDefaultAsync(x => x.Id == request.ExpenseId, cancellationToken);
        if (expense == null)
            return new ApiResponse("Expense not found");

        var user = await _dbContext.Set<User>()
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

        _dbContext.Set<Expense>().Update(expense);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse();
    }
    
    public async Task<ApiResponse> Handle (RejectExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await _dbContext.Set<Expense>().FirstOrDefaultAsync(x => x.Id == request.ExpenseId, cancellationToken);
        if (expense == null)
            return new ApiResponse("Expense not found");

        expense.RejectionReason = request.RejectionReason;
        expense.ExpenseStatus = ExpenseStatus.Rejected;

        _dbContext.Set<Expense>().Update(expense);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse();
    }
    
    public async Task<ApiResponse> Handle(CancelExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await _dbContext.Set<Expense>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (expense == null)
            return new ApiResponse("Expense not found");

        expense.IsActive = false;

        _dbContext.Set<Expense>().Update(expense);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse();
    }


     
}
