using ExpenseManager.Base.ApiResponse;
using ExpenseManager.Schema;
using MediatR;

namespace ExpenseManager.Api.Impl.Cqrs;

public record GetAllAccountHistoryQuery : IRequest<ApiResponse<List<AccountHistoryResponse>>>;
public record GetAccountHistoryByIdQuery(int Id) : IRequest<ApiResponse<AccountHistoryResponse>>;
public record DeleteAccountHistoryCommand(int Id) : IRequest<ApiResponse>;