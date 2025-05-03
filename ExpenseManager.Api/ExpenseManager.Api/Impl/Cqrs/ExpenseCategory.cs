using ExpenseManager.Base.ApiResponse;
using ExpenseManager.Schema;
using MediatR;

namespace ExpenseManager.Api.Impl.Cqrs;

public record GetAllExpenseCategoriesQuery : IRequest<ApiResponse<List<ExpenseCategoryResponse>>>;
public record GetExpenseCategoriesByIdQuery(int Id) : IRequest<ApiResponse<ExpenseCategoryResponse>>;
public record CreateExpenseCategoryCommand(ExpenseCategoryRequest ExpenseCategory) : IRequest<ApiResponse<ExpenseCategoryResponse>>;
public record UpdateExpenseCategoryCommand(int Id, ExpenseCategoryRequest ExpenseCategory) : IRequest<ApiResponse>;
public record DeleteExpenseCategoryCommand(int Id) : IRequest<ApiResponse>;


