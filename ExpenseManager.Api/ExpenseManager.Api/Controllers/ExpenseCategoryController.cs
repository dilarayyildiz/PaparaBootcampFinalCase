using ExpenseManager.Api.Impl.Cqrs;
using ExpenseManager.Base.ApiResponse;
using ExpenseManager.Schema;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")] // sadece admin erişebilir
public class ExpenseCategoryController : ControllerBase
{
    private readonly IMediator mediator;

    public ExpenseCategoryController(IMediator mediator)
    {
        this.mediator = mediator;
    }
    [HttpGet]
    public async Task<ApiResponse<List<ExpenseCategoryResponse>>> GetAllExpenseCategories()
    {
        var query = new GetAllExpenseCategoriesQuery();
        var result = await mediator.Send(query);
        return result;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<ExpenseCategoryResponse>> GetExpenseCategoryById(int id)
    {
        var query = new GetExpenseCategoriesByIdQuery(id);
        var result = await mediator.Send(query);
        return result;
    }

    [HttpPost]
    public async Task<ApiResponse<ExpenseCategoryResponse>> CreateExpenseCategory([FromBody] ExpenseCategoryRequest request)
    {
        var command = new CreateExpenseCategoryCommand(request);
        var result = await mediator.Send(command);
        return result;
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse> UpdateExpenseCategory(int id, [FromBody] ExpenseCategoryRequest request)
    {
        var command = new UpdateExpenseCategoryCommand(id, request);
        var result = await mediator.Send(command);
        return result;
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> DeleteExpenseCategory(int id)
    {
        var command = new DeleteExpenseCategoryCommand(id);
        var result = await mediator.Send(command);
        return result;
    }
}