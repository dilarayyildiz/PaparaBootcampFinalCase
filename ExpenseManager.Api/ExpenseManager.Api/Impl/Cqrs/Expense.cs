using ExpenseManager.Base.ApiResponse;
using ExpenseManager.Schema;
using MediatR;

namespace ExpenseManager.Api.Impl.Cqrs;

public record GetAllExpenseQuery : IRequest<ApiResponse<List<ExpenseResponse>>>;
public record GetExpenseByIdQuery(int Id) : IRequest<ApiResponse<ExpenseResponse>>;
public record CreateExpenseCommand(ExpenseRequest Expense) : IRequest<ApiResponse<ExpenseResponse>>;
public record UpdateExpenseCommand(int Id, ExpenseRequest Expense) : IRequest<ApiResponse>;
public record ApproveExpenseCommand(int ExpenseId) : IRequest<ApiResponse>;
public record RejectExpenseCommand(int ExpenseId, string RejectionReason) : IRequest<ApiResponse>;
public record CancelExpenseCommand(int Id) : IRequest<ApiResponse>;