using AutoMapper;
using ExpenseManager.Api.Entities;
using ExpenseManager.Api.Impl.Cqrs;
using ExpenseManager.Api.Impl.UnitOfWork;
using ExpenseManager.Api.Services.Cashe;
using ExpenseManager.Api.Services.RedisCache;
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
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;

    public ExpenseCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
    {
        this.unitOfWork = unitOfWork;
        this._mapper = mapper;
        this._cacheService = cacheService;
    }

    public async Task<ApiResponse<ExpenseCategoryResponse>> Handle(CreateExpenseCategoryCommand request, CancellationToken cancellationToken)
    {
        var existing = await unitOfWork.ExpenseCategoryRepository.FirstOrDefaultAsync(x => x.Name == request.ExpenseCategory.Name);
        if (existing != null)
            return new ApiResponse<ExpenseCategoryResponse>("This category name already exists");

        var category = _mapper.Map<ExpenseCategory>(request.ExpenseCategory);
        category.IsActive = true;

        await unitOfWork.ExpenseCategoryRepository.AddAsync(category, cancellationToken);
        await unitOfWork.Complete(cancellationToken);

        // Redis cache temizle
        await _cacheService.RemoveAsync("expense_categories");

        var response = _mapper.Map<ExpenseCategoryResponse>(category);
        return new ApiResponse<ExpenseCategoryResponse>(response);
    }

    public async Task<ApiResponse> Handle(UpdateExpenseCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await unitOfWork.ExpenseCategoryRepository.FirstOrDefaultAsync(x => x.Id == request.Id);
        if (category == null)
            return new ApiResponse("Category not found");

        category.Name = request.ExpenseCategory.Name;

        unitOfWork.ExpenseCategoryRepository.Update(category);
        await unitOfWork.Complete(cancellationToken);

        // Redis cache temizle
        await _cacheService.RemoveAsync("expense_categories");

        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(DeleteExpenseCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await unitOfWork.ExpenseCategoryRepository
            .GetWithIncludesAsync(
                x => x.Id == request.Id,
                cancellationToken,
                c => c.Expenses);

        if (category == null || !category.IsActive)
            return new ApiResponse("Category already deleted");

        if (category.Expenses.Any(e => e.IsActive))
            return new ApiResponse("Cannot delete category with active expenses.");

        category.IsActive = false;

        unitOfWork.ExpenseCategoryRepository.Update(category);
        await unitOfWork.Complete(cancellationToken);

        // Redis cache temizle
        await _cacheService.RemoveAsync("expense_categories");

        return new ApiResponse();
    }
}
