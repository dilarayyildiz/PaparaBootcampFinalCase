using ExpenseManager.Base.ApiResponse;
using ExpenseManager.Schema;
using MediatR;

namespace ExpenseManager.Api.Impl.Cqrs;

public record GetAllExpenseQuery : IRequest<ApiResponse<List<ExpenseResponse>>>;
public record GetExpenseByIdQuery(int Id) : IRequest<ApiResponse<ExpenseResponse>>;
public record CreateExpenseCommand(ExpenseRequest User) : IRequest<ApiResponse<ExpenseResponse>>;
public record UpdateExpenseCommand(int Id, ExpenseRequest User) : IRequest<ApiResponse>;
public record DeleteExpenseCommand(int Id) : IRequest<ApiResponse>;