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
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountHistoryQueryHandler(ExpenseManagerDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ApiResponse<List<AccountHistoryResponse>>> Handle(GetAllAccountHistoriesQuery request, CancellationToken cancellationToken)
    {
        //Get userId from claims(ApiSession)
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.Claims
            .FirstOrDefault(c => c.Type == "UserId")?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim))
        {
            return new ApiResponse<List<AccountHistoryResponse>>("Unauthorized or missing UserId claim");
        } 
        var userId = int.Parse(userIdClaim);
        
        //Get user IBAN
        var user = await dbContext.Set<User>()
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        
        //Get account histories with toIBAN
        var histories = await dbContext.Set<AccountHistory>()
            .Where(x => x.IsActive && x.ToIBAN == user.IBAN)
            .ToListAsync(cancellationToken);

        var mapped = mapper.Map<List<AccountHistoryResponse>>(histories);
        return new ApiResponse<List<AccountHistoryResponse>>(mapped);
    }

    public async Task<ApiResponse<AccountHistoryResponse>> Handle(GetAccountHistoryByIdQuery request, CancellationToken cancellationToken)
    {
        //Get userId from claims(ApiSession)
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.Claims
            .FirstOrDefault(c => c.Type == "UserId")?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim))
        {
            return new ApiResponse<AccountHistoryResponse>("Unauthorized or missing UserId claim");
        } 
        var userId = int.Parse(userIdClaim);
        
        //Get user IBAN
        var user = await dbContext.Set<User>()
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        var predicate = PredicateBuilder.New<AccountHistory>(true);
        predicate = predicate.And(x => x.Id == request.Id && x.IsActive && x.ToIBAN == user.IBAN);

        var history = await dbContext.Set<AccountHistory>().FirstOrDefaultAsync(predicate, cancellationToken);
        if (history == null)
            return new ApiResponse<AccountHistoryResponse>("Account history not found");

        var mapped = mapper.Map<AccountHistoryResponse>(history);
        return new ApiResponse<AccountHistoryResponse>(mapped);
    }
}