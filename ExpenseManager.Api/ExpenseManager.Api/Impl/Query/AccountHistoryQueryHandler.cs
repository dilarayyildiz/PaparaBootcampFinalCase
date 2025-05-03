using AutoMapper;
using ExpenseManager.Api.Entities;
using ExpenseManager.Api.Impl.Cqrs;
using ExpenseManager.Base.ApiResponse;
using Microsoft.EntityFrameworkCore;
using ExpenseManager.Schema;
using LinqKit;
using MediatR;

namespace ExpenseManager.Api.Impl.Query;

public class AccountHistoryQueryHandler:
    IRequestHandler<GetAllAccountHistoriesQuery, ApiResponse<List<AccountHistoryResponse>>>,
    IRequestHandler<GetAccountHistoryByIdQuery, ApiResponse<AccountHistoryResponse>>
{
    private readonly ExpenseManagerDbContext dbContext;
    private readonly IMapper mapper;

    public AccountHistoryQueryHandler(ExpenseManagerDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<List<AccountHistoryResponse>>> Handle(GetAllAccountHistoriesQuery request, CancellationToken cancellationToken)
    {
        var histories = await dbContext.Set<AccountHistory>()
            .Where(x => x.IsActive)
            .ToListAsync(cancellationToken);

        var mapped = mapper.Map<List<AccountHistoryResponse>>(histories);
        return new ApiResponse<List<AccountHistoryResponse>>(mapped);
    }

    public async Task<ApiResponse<AccountHistoryResponse>> Handle(GetAccountHistoryByIdQuery request, CancellationToken cancellationToken)
    {
        var predicate = PredicateBuilder.New<AccountHistory>(true);
        predicate = predicate.And(x => x.Id == request.Id && x.IsActive);

        var history = await dbContext.Set<AccountHistory>().FirstOrDefaultAsync(predicate, cancellationToken);
        if (history == null)
            return new ApiResponse<AccountHistoryResponse>("Account history not found");

        var mapped = mapper.Map<AccountHistoryResponse>(history);
        return new ApiResponse<AccountHistoryResponse>(mapped);
    }
}