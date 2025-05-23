using ExpenseManager.Api.Request;
using ExpenseManager.Base.ApiResponse;
using ExpenseManager.Schema;
using MediatR;

namespace ExpenseManager.Api.Impl.Cqrs;

public record GetAllExpenseQuery : IRequest<ApiResponse<List<ExpenseResponse>>>;
public record GetExpenseByIdQuery(int Id) : IRequest<ApiResponse<ExpenseResponse>>;
public record GetExpenseByUser : IRequest<ApiResponse<List<ExpenseResponse>>>;
public record CreateExpenseCommand(string ReceptUrl,CreateExpenseRequest Expense) : IRequest<ApiResponse<CreateExpenseResponse>>;
public record UpdateExpenseCommand(int Id, ExpenseRequest Expense) : IRequest<ApiResponse>;
public record ApproveExpenseCommand(int ExpenseId) : IRequest<ApiResponse>;
public record RejectExpenseCommand(int ExpenseId, string RejectionReason) : IRequest<ApiResponse>;
public record CancelExpenseCommand(int Id) : IRequest<ApiResponse>;