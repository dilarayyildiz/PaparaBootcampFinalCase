using AutoMapper;
using ExpenseManager.Api.Entities;
using ExpenseManager.Api.Impl.Cqrs;
using ExpenseManager.Base.ApiResponse;
using Microsoft.EntityFrameworkCore;
using ExpenseManager.Schema;
using MediatR;

namespace ExpenseManager.Api.Impl.Command;

public class ExpenseCategoryCommandHandler :
    IRequestHandler<CreateExpenseCategoryCommand, ApiResponse<ExpenseCategoryResponse>>,
    IRequestHandler<UpdateExpenseCategoryCommand, ApiResponse>,
    IRequestHandler<DeleteExpenseCategoryCommand, ApiResponse>
{
    private readonly ExpenseManagerDbContext _dbContext;
    private readonly IMapper _mapper;

    public ExpenseCategoryCommandHandler(ExpenseManagerDbContext dbContext, IMapper mapper)
    {
        this._dbContext = dbContext;
        this._mapper = mapper;
    }

    public async Task<ApiResponse<ExpenseCategoryResponse>> Handle(CreateExpenseCategoryCommand request, CancellationToken cancellationToken)
    {
        var existing = await _dbContext.Set<ExpenseCategory>().FirstOrDefaultAsync(x => x.Name == request.ExpenseCategory.Name, cancellationToken);
        if (existing != null)
            return new ApiResponse<ExpenseCategoryResponse>("This category name already exists");

        var category = _mapper.Map<ExpenseCategory>(request.ExpenseCategory);
        category.IsActive = true;

        await _dbContext.Set<ExpenseCategory>().AddAsync(category, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var response = _mapper.Map<ExpenseCategoryResponse>(category);
        return new ApiResponse<ExpenseCategoryResponse>(response);
    }

    public async Task<ApiResponse> Handle(UpdateExpenseCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _dbContext.Set<ExpenseCategory>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (category == null)
            return new ApiResponse("Category not found");

        category.Name = request.ExpenseCategory.Name;

        _dbContext.Set<ExpenseCategory>().Update(category);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(DeleteExpenseCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _dbContext.Set<ExpenseCategory>()
            .Include(c => c.Expenses.Where(e => e.IsActive))
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (category == null || category.IsActive == false)
            return new ApiResponse("Category already deleted");

        if (category.Expenses.Any())
            return new ApiResponse("Cannot delete category with active expenses.");

        category.IsActive = false;

        _dbContext.Set<ExpenseCategory>().Update(category);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse();
    }
    
}
