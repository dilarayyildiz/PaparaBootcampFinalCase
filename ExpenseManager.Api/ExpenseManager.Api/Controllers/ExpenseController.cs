using ExpenseManager.Api.Impl.Cqrs;
using ExpenseManager.Base.ApiResponse;
using ExpenseManager.Schema;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class ExpenseController : ControllerBase
{
    private readonly IMediator mediator;

    public ExpenseController(IMediator mediator)
    {
        this.mediator = mediator;
    }
    
    [HttpGet]
    public async Task<ApiResponse<List<ExpenseResponse>>> GetAllExpenses()
    {
        var query = new GetAllExpenseQuery();
        var result = await mediator.Send(query);
        return result;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<ExpenseResponse>> GetExpenseById(int id)
    {
        var query = new GetExpenseByIdQuery(id);
        var result = await mediator.Send(query);
        return result;
    }

    [HttpPost]
    public async Task<ApiResponse<ExpenseResponse>> CreateExpense([FromBody] ExpenseRequest request)
    {
        var command = new CreateExpenseCommand(request);
        var result = await mediator.Send(command);
        return result;
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse> UpdateExpense(int id, [FromBody] ExpenseRequest request)
    {
        var command = new UpdateExpenseCommand(id, request);
        var result = await mediator.Send(command);
        return result;
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> DeleteExpense(int id)
    {
        var command = new DeleteExpenseCommand(id);
        var result = await mediator.Send(command);
        return result;
    }
}