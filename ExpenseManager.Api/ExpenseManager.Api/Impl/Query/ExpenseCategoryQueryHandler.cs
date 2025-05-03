using AutoMapper;
using ExpenseManager.Api.Entities;
using ExpenseManager.Api.Impl.Cqrs;
using ExpenseManager.Base.ApiResponse;
using Microsoft.EntityFrameworkCore;
using ExpenseManager.Schema;
using LinqKit;
using MediatR;

namespace ExpenseManager.Api.Impl.Query;

public class ExpenseCategoryQueryHandler :
    IRequestHandler<GetAllExpenseCategoriesQuery, ApiResponse<List<ExpenseCategoryResponse>>>,
    IRequestHandler<GetExpenseCategoriesByIdQuery, ApiResponse<ExpenseCategoryResponse>>
{
    private readonly ExpenseManagerDbContext _context;
    private readonly IMapper _mapper;

    public ExpenseCategoryQueryHandler(ExpenseManagerDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<ExpenseCategoryResponse>>> Handle(GetAllExpenseCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _context.Set<ExpenseCategory>()
            .Where(x => x.IsActive)
            .ToListAsync(cancellationToken);

        var mapped = _mapper.Map<List<ExpenseCategoryResponse>>(categories);
        return new ApiResponse<List<ExpenseCategoryResponse>>(mapped);
    }

    public async Task<ApiResponse<ExpenseCategoryResponse>> Handle(GetExpenseCategoriesByIdQuery request, CancellationToken cancellationToken)
    {
        var predicate = PredicateBuilder.New<ExpenseCategory>(true);
        predicate = predicate.And(x => x.Id == request.Id && x.IsActive);

        var category = await _context.Set<ExpenseCategory>()
            .FirstOrDefaultAsync(predicate, cancellationToken);

        if (category == null)
            return new ApiResponse<ExpenseCategoryResponse>("Category not found");

        var mapped = _mapper.Map<ExpenseCategoryResponse>(category);
        return new ApiResponse<ExpenseCategoryResponse>(mapped);
    }
}