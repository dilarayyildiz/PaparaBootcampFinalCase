using AutoMapper;
using ExpenseManager.Api.Entities;
using ExpenseManager.Api.Impl.Cqrs;
using ExpenseManager.Base.ApiResponse;
using ExpenseManager.Schema;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using MediatR;

namespace ExpenseManager.Api.Impl.Query;

public class ExpenseQueryHandler :
    IRequestHandler<GetAllExpenseQuery, ApiResponse<List<ExpenseResponse>>>,
    IRequestHandler<GetExpenseByIdQuery, ApiResponse<ExpenseResponse>>
{
    private readonly ExpenseManagerDbContext _context;
    private readonly IMapper _mapper;

    public ExpenseQueryHandler(ExpenseManagerDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<ExpenseResponse>>> Handle(GetAllExpenseQuery request, CancellationToken cancellationToken)
    {
        var expenses = await _context.Set<Expense>()
            .Include(e => e.User)
            .Include(e => e.Category)
            .ToListAsync(cancellationToken);

        var mapped = _mapper.Map<List<ExpenseResponse>>(expenses);
        return new ApiResponse<List<ExpenseResponse>>(mapped);
    }

    public async Task<ApiResponse<ExpenseResponse>> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
    {
        var predicate = PredicateBuilder.New<Expense>(true);
        predicate = predicate.And(x => x.Id == request.Id);

        var expense = await _context.Set<Expense>()
            .Include(e => e.User)
            .Include(e => e.Category)
            .FirstOrDefaultAsync(predicate, cancellationToken);

        var mapped = _mapper.Map<ExpenseResponse>(expense);
        return new ApiResponse<ExpenseResponse>(mapped);
    }
}