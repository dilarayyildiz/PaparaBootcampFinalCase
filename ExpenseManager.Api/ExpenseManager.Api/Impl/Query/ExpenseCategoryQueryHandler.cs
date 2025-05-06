using AutoMapper;
using ExpenseManager.Api.Entities;
using ExpenseManager.Api.Impl.Cqrs;
using ExpenseManager.Api.Impl.UnitOfWork;
using ExpenseManager.Api.Services.Cashe;
using ExpenseManager.Api.Services.RedisCache;
using ExpenseManager.Base.ApiResponse;
using ExpenseManager.Schema;
using LinqKit;
using MediatR;

namespace ExpenseManager.Api.Impl.Query;

public class ExpenseCategoryQueryHandler :
    IRequestHandler<GetAllExpenseCategoriesQuery, ApiResponse<List<ExpenseCategoryResponse>>>,
    IRequestHandler<GetExpenseCategoriesByIdQuery, ApiResponse<ExpenseCategoryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;

    public ExpenseCategoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cacheService = cacheService;
    }

    public async Task<ApiResponse<List<ExpenseCategoryResponse>>> Handle(GetAllExpenseCategoriesQuery request, CancellationToken cancellationToken)
    {
        // Önce cache kontrolü
        var cachedCategories = await _cacheService.GetAsync<List<ExpenseCategoryResponse>>("expense_categories");
        if (cachedCategories != null)
        {
            return new ApiResponse<List<ExpenseCategoryResponse>>(cachedCategories);
        }

        // Yoksa DB'den çek
        var categories = await _unitOfWork.ExpenseCategoryRepository
            .Where(x => x.IsActive);

        var mapped = _mapper.Map<List<ExpenseCategoryResponse>>(categories);

        // Cache’e kaydet
        await _cacheService.SetAsync("expense_categories", mapped, TimeSpan.FromMinutes(10));

        return new ApiResponse<List<ExpenseCategoryResponse>>(mapped);
    }

    public async Task<ApiResponse<ExpenseCategoryResponse>> Handle(GetExpenseCategoriesByIdQuery request, CancellationToken cancellationToken)
    {
        var predicate = PredicateBuilder.New<ExpenseCategory>(true);
        predicate = predicate.And(x => x.Id == request.Id && x.IsActive);

        var category = await _unitOfWork.ExpenseCategoryRepository.FirstOrDefaultAsync(predicate);
        if (category == null)
            return new ApiResponse<ExpenseCategoryResponse>("Category not found");

        var mapped = _mapper.Map<ExpenseCategoryResponse>(category);
        return new ApiResponse<ExpenseCategoryResponse>(mapped);
    }
}
