using ExpenseManager.Base.ApiResponse;
using ExpenseManager.Schema;
using MediatR;

namespace ExpenseManager.Api.Impl.Cqrs;

public record GetAllAccountHistoriesQuery : IRequest<ApiResponse<List<AccountHistoryResponse>>>;
public record GetAccountHistoryByIdQuery(int Id) : IRequest<ApiResponse<AccountHistoryResponse>>;
