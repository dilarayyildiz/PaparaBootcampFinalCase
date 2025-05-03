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
    private readonly ExpenseManagerDbContext dbContext;
    private readonly IMapper mapper;

    public ExpenseCategoryCommandHandler(ExpenseManagerDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<ExpenseCategoryResponse>> Handle(CreateExpenseCategoryCommand request, CancellationToken cancellationToken)
    {
        var existing = await dbContext.Set<ExpenseCategory>().FirstOrDefaultAsync(x => x.Name == request.ExpenseCategory.Name, cancellationToken);
        if (existing != null)
            return new ApiResponse<ExpenseCategoryResponse>("This category name already exists");

        var category = mapper.Map<ExpenseCategory>(request.ExpenseCategory);
        category.IsActive = true;

        await dbContext.Set<ExpenseCategory>().AddAsync(category, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = mapper.Map<ExpenseCategoryResponse>(category);
        return new ApiResponse<ExpenseCategoryResponse>(response);
    }

    public async Task<ApiResponse> Handle(UpdateExpenseCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await dbContext.Set<ExpenseCategory>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (category == null)
            return new ApiResponse("Category not found");

        category.Name = request.ExpenseCategory.Name;

        dbContext.Set<ExpenseCategory>().Update(category);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(DeleteExpenseCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await dbContext.Set<ExpenseCategory>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (category == null || category.IsActive == false)
            return new ApiResponse("Category already deleted");

        category.IsActive = false;

        dbContext.Set<ExpenseCategory>().Update(category);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse();
    }
    
}
