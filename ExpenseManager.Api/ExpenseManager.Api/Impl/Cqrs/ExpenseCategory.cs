using ExpenseManager.Base.ApiResponse;
using ExpenseManager.Schema;
using MediatR;

namespace ExpenseManager.Api.Impl.Cqrs;

public record GetAllExpenseCategoriesQuery : IRequest<ApiResponse<List<ExpenseCategoryResponse>>>;
public record GetExpenseCategoriesByIdQuery(int Id) : IRequest<ApiResponse<ExpenseCategoryResponse>>;
public record CreateExpenseCategoriesCommand(ExpenseCategoryRequest User) : IRequest<ApiResponse<ExpenseCategoryResponse>>;
public record UpdateExpenseCategoriesCommand(int Id, ExpenseCategoryRequest User) : IRequest<ApiResponse>;
public record DeleteExpenseCategoriesCommand(int Id) : IRequest<ApiResponse>;


